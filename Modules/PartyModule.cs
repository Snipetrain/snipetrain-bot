using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace snipetrain_bot.Modules
{
    [Group("event")]
    public class PartyModule : ModuleBase
    {
        private readonly IPartyService _partyService;
        private readonly DiscordRunner _runner;
        private IConfiguration _config;
        public PartyModule(IPartyService partyService, DiscordRunner runner)
        {
            _partyService = partyService;
            _runner = runner;
        }

        [Command()]
        public async Task InitiateParty([Remainder] string region)
        {
            try
            {
                var dailies = await _partyService.GetDailyPartiesAsync();

                if (region != "EU" || region != "US") throw new PartyException("Usage: !event <NA|EU>");
                if (region == "EU")
                {
                    if (dailies.Any(x => x.State == PartyState.VOTING)) throw new PartyException("There is already a vote going on right now!");
                    if (dailies.Count >= 2) throw new PartyException("There has been already 2 events today! Try again tomorrow.");

                    var msg = await _runner.SendMessage("Do you Want a SvS Match Right Now ?", 747139803711012884);
                    var emote = Emote.Parse(_config.GetSection("discord").GetSection("emotes")["vote"]);
                    var datetime = DateTimeOffset.Now;
                    await msg.AddReactionAsync(emote, null);

                    await _partyService.AddPartyAsync(new PartySchema
                    {
                        CreatedDate = datetime,
                        InitiatedBy = Context.User.Username,
                        Region = region,
                        State = PartyState.VOTING,
                        ExpiryDate = DateTimeOffset.Now + TimeSpan.FromMinutes(30),
                        MessageId = msg.Id
                    });

                }
                else if (region == "US")
                {
                    if (dailies.Any(x => x.State == PartyState.VOTING)) throw new PartyException("There is already a vote going on right now!");
                    if (dailies.Count >= 2) throw new PartyException("There has been already 2 events today! Try again tomorrow.");

                    var msg = await _runner.SendMessage("Do you Want a SvS Match Right Now ?", 700712690288427129);
                    var emote = Emote.Parse(_config.GetSection("discord").GetSection("emotes")["vote"]);
                    var datetime = DateTimeOffset.Now;
                    await msg.AddReactionAsync(emote, null);

                    await _partyService.AddPartyAsync(new PartySchema
                    {
                        CreatedDate = datetime,
                        InitiatedBy = Context.User.Username,
                        Region = region,
                        State = PartyState.VOTING,
                        ExpiryDate = DateTimeOffset.Now + TimeSpan.FromMinutes(30),
                        MessageId = msg.Id
                    });
                }

                // TODO here:
                // Send message or embed to Appropriate channel, e.g. EU = #snipetrain-tf2-eu (Done)
                // Start monitoring reactions with any emote and wait for count of 6. (Done)
                // if count >= 6 then send @everyone ping to #announcements with quick connect link to game server (Done)
                // In the future we can add steam group automatic announcement (On Hold)

            }
            catch (PartyException e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error trying to add a event :: Check Logs");
            }
        }
    }
}
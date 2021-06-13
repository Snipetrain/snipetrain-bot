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
        private readonly IConfiguration _config;
        public PartyModule(IPartyService partyService, DiscordRunner runner,IConfiguration config)
        {
            _partyService = partyService;
            _runner = runner;
            _config = config;
        }

        [Command()]
        public async Task InitiateParty([Remainder] string region)
        {
            try
            {
                var dailies = (await _partyService.GetPartiesAsync())
                    .Where(x => x.CreatedDate.TimeOfDay < TimeSpan.FromDays(1))
                    .ToList();
                
                if (region != "EU" || region != "US") throw new PartyException("Usage: !event <NA|EU>");
                
                if (region == "EU")
                {
                    if (dailies.Any(x => x.State == PartyState.Voting)) throw new PartyException("There is already a vote going on right now!");
                    if (dailies.Count >= 2) throw new PartyException("There has been already 2 events today! Try again tomorrow.");

                    var channelEU = ulong.Parse(_config.GetSection("discord").GetSection("channels")["snipetrain-tf2-eu"]);
                    var msg = await _runner.SendMessage("Do you Want a SvS Match Right Now ?", channelEU);
                    var emoji = new Emoji("\uD83D\uDC94");
                    
                    await msg.AddReactionAsync(emoji, null);

                    await _partyService.AddPartyAsync(new PartySchema
                    {
                        CreatedDate = DateTime.UtcNow,
                        InitiatedBy = Context.User.Username,
                        Region = region,
                        State = PartyState.Voting,
                        ExpiryDate = DateTime.UtcNow + TimeSpan.FromMinutes(1),
                        MessageId = msg.Id
                    });
                    
                }
                else if (region == "US")
                {
                    if (dailies.Any(x => x.State == PartyState.Voting)) throw new PartyException("There is already a vote going on right now!");
                    if (dailies.Count >= 2) throw new PartyException("There has been already 2 events today! Try again tomorrow.");
                    
                    var channelUS = ulong.Parse(_config.GetSection("discord").GetSection("channels")["snipetrain-tf2-us"]);
                    var msg = await _runner.SendMessage("Do you Want a SvS Match Right Now ?", channelUS);
                    var emoji = new Emoji("\uD83D\uDC94");

                    await msg.AddReactionAsync(emoji, null);

                    await _partyService.AddPartyAsync(new PartySchema
                    {
                        CreatedDate = DateTime.UtcNow,
                        InitiatedBy = Context.User.Username,
                        Region = region,
                        State = PartyState.Voting,
                        ExpiryDate = DateTime.UtcNow + TimeSpan.FromMinutes(1),
                        MessageId = msg.Id
                    });
                }

                // TODO here:
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
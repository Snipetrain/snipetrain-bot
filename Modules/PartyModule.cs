using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;
using System.Linq;

namespace snipetrain_bot.Modules
{
    [Group("event")]
    public class PartyModule : ModuleBase
    {
        private readonly IPartyService _partyService;
        public PartyModule(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [Command()]
        public async Task InitiateParty([Remainder] string region)
        {
            try
            {

                if (region != "EU" || region != "US") throw new PartyException("Usage: !event <NA|EU>");


                var dailies = await _partyService.GetDailyPartiesAsync();

                if (dailies.Count < 2) throw new PartyException("There has been already 2 events today! Try again tomorrow.");
  
                if (!dailies.Any(x => x.State == PartyState.VOTING)) throw new PartyException("There is already a vote going on right now!");

                // TODO here:
                // Send message or embed to Appropriate channel, e.g. EU = #snipetrain-tf2-eu
                // Start monitoring reactions with any emote and wait for count of 6.
                // if count >= 6 then send @everyone ping to #announcements with quick connect link to game server
                // In the future we can add steam group automatic announcement

                await _partyService.AddPartyAsync(new PartySchema
                {
                    CreatedDate = DateTimeOffset.Now,
                    InitiatedBy = Context.User.Username,
                    Region = region,
                    State = PartyState.VOTING,
                    ExpiryDate = DateTimeOffset.Now + TimeSpan.FromMinutes(30)
                });
                
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
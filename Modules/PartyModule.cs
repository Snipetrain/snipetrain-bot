using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;

namespace snipetrain_bot.Modules
{
    [Group("party")]
    public class EventModule : ModuleBase
    {
        private readonly IPartyService _partyService;
        System.Timers.Timer t = new System.Timers.Timer();
        public EventModule(IPartyService partyService)
        {
            _partyService = partyService;
        }
        [Command("add")]
        public async Task addParty(string prize, int addDaysNum, [Remainder] string message)
        {
            try
            {
                var user = Context.User.ToString();
                var eventDay = DateTime.Now.AddDays(addDaysNum);
                var andate = DateTime.Now;

                var party = new PartySchema
                {
                    Prize = prize,
                    Message = message,
                    AnDate = andate,
                    Name = user,
                    EventDay = eventDay
                };

                await _partyService.AddPartyAsync(party); // Adds Party to DB

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error trying to add a event :: Check Logs");
            }
        }
        [Command("delete")]
        public async Task deleteParty([Remainder] string id)
        {
            try
            {
                var party = await _partyService.GetPartyAsync(id);

                if (party == null)
                    await ReplyAsync("Party Doesnt Exist in the DB");

                
                await _partyService.RemovePartyAsync(id);

                await ReplyAsync("Deleted Party from thr DB");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                await ReplyAsync("Error While Trying to Delete the Doc from the DB,Please Check the logs");
            }
        }
    }
}
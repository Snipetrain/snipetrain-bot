using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;

namespace snipetrain_bot.Modules
{
    [Group("party")]
    public class PartyModule : ModuleBase
    {
        private readonly IPartyService _partyService;
        public PartyModule(IPartyService partyService)
        {
            _partyService = partyService;
        }
        [Command("add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task addParty(string prize,string region, int addDaysNum, [Remainder] string message)
        {
            try
            {
                var user = Context.User.ToString();
                var eventDay = DateTimeOffset.Now.AddDays(addDaysNum);
                var andate = DateTimeOffset.Now;

                var party = new PartySchema
                {
                    Prize = prize,
                    Message = message,
                    AnDate = andate,
                    Name = user,
                    EventDay = eventDay,
                    Region = region
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
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task deleteParty([Remainder] string id)
        {
            try
            {
                var party = await _partyService.GetPartyAsync(id);

                if (party == null)
                    throw new DocDoesntExistException("Doc doesn't exist in DB!");


                await _partyService.RemovePartyAsync(id);

                await ReplyAsync("Deleted Party from thr DB");
            }
            catch(DocDoesntExistException e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync("Error While Trying to Delete the Doc from the DB,Please Check the logs");
            }
        }
    }
}
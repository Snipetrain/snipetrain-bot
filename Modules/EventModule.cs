using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;

namespace snipetrain_bot.Modules
{
    [Group("event")]
    public class EventModule : ModuleBase
    {
        private readonly IEventService _eventservice;
        System.Timers.Timer t = new System.Timers.Timer();
        public EventModule(IEventService eventService)
        {
            _eventservice = eventService;
        }
        [Command("add")]
        public async Task addEvent(string prize, int addDaysNum, [Remainder] string message)
        {
            try
            {
                var user = Context.User.ToString();
                var eventDay = DateTime.Now.AddDays(addDaysNum);
                var andate = DateTime.Now.ToString();

                var events = new EventSchema
                {
                    Prize = prize,
                    Message = message,
                    AnDate = andate,
                    Name = user,
                    EventDay = eventDay
                };
                await _eventservice.AddEventAsync(events);

                _eventservice.AddEventToList(eventDay);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error trying to add a event :: Check Logs");
            }
        }
        [Command("delete")]
        public async Task deletEvent([Remainder] string id)
        {
            try
            {
                var docid = await _eventservice.GetDocAsync(id);

                if (docid == null)
                    await ReplyAsync("Event Doesnt Exist in the DB");

                await _eventservice.DeleteDocAsync(id);

                await ReplyAsync("Deleted the Doc from thr DB");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                await ReplyAsync("Error While Trying to Delete the Doc from the DB,Please Check the logs");
            }
        }
    }
}
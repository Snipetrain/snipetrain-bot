using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace snipetrain_bot.Events
{
    public class EventsList
    {
        private readonly DiscordRunner _runner;
        private readonly IConfiguration _config;
        private readonly DateTime _date;
        public EventsList(DiscordRunner runner, IConfiguration config, DateTime date)
        {
            _config = config;
            _date = date;
            _runner = runner;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;
using snipetrain_bot.Events;

namespace snipetrain_bot.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<EventSchema> _Event;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private List<EventsList> Events = new List<EventsList>(); 

        public EventService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("connectionStrings")["snipetrain"]);
            var database = client.GetDatabase("SnipeEvent");

            _Event = database.GetCollection<EventSchema>("sn_Event");
        }
        public async Task<List<EventSchema>> GetDocAsync()
        {
            return (await _Event.FindAsync(s => true)).ToList();
        }
        public async Task<EventSchema> GetDocAsync(string id)
        {
            return await (await _Event.FindAsync(s => s.Id == id)).FirstOrDefaultAsync();
        }
        public async Task AddEventAsync(EventSchema model)
        {
            await _Event.InsertOneAsync(model);
        }
        public async Task DeleteDocAsync(string id)
        {
            await _Event.DeleteOneAsync(s => s.Id == id);
        }
        public void AddEventToList(DateTime eventDay)
        {
            var runner = _serviceProvider.GetService(typeof(DiscordRunner)) as DiscordRunner;
            Events.Add(new EventsList(runner, _config,eventDay)); 
        }
    }
}
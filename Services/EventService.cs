using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<EventSchema> _Event;

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
        
    }
}
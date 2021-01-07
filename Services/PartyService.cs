using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;
using snipetrain_bot.Events;

namespace snipetrain_bot.Services
{
    public class PartyService : IPartyService
    {
        private readonly IMongoCollection<PartySchema> _parties;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public PartyService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("connectionStrings")["snipetrain"]);
            var database = client.GetDatabase("snipetrain");

            _parties = database.GetCollection<PartySchema>("sn_parties");
        }
        public async Task<List<PartySchema>> GetPartiesAsync()
        {
            return (await _parties.FindAsync(s => true)).ToList();
        }
        public async Task<PartySchema> GetPartyAsync(string id)
        {
            return await (await _parties.FindAsync(s => s.Id == id)).FirstOrDefaultAsync();
        }
        public async Task AddPartyAsync(PartySchema model)
        {
            await _parties.InsertOneAsync(model);
        }
        public async Task RemovePartyAsync(string id)
        {
            await _parties.DeleteOneAsync(s => s.Id == id);
        }
    }
}
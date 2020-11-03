using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class StreamersService : IStreamersService
    {
        private readonly IMongoCollection<StreamersSchema> _streamers;

        public StreamersService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("connectionStrings")["snipetrain"]);
            var database = client.GetDatabase("snipetrain");

            _streamers = database.GetCollection<StreamersSchema>("sn_streamers");
        }

        public async Task<List<StreamersSchema>> GetStreamersAsync()
        {
            return (await _streamers.FindAsync(s => true)).ToList();
        }

        public async Task<StreamersSchema> GetStreamerAsync(string name)
        {
            return await (await _streamers.FindAsync(s => s.Name == name)).FirstOrDefaultAsync();
        }

        public async Task AddStreamerAsync(StreamersSchema model)
        {
            await _streamers.InsertOneAsync(model);
        }

        public async Task DeleteStreamerAsync(string name)
        {
            await _streamers.DeleteOneAsync(s => s.Name == name);
        }

    }
}
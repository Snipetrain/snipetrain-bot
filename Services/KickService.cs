using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class KickService : IKickService
    {
        private readonly IMongoCollection<KickSchema> _Kick;
        public KickService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("connectionStrings")["snipetrain"]);
            var database = client.GetDatabase("UserInteractions");

            _Kick = database.GetCollection<KickSchema>("Kick");
        }
        public async Task AddKickAsync(KickSchema model)
        {
            await _Kick.InsertOneAsync(model);
        }
    }
}
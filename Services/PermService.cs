using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using snipetrain_bot.Models;
using Discord;


namespace snipetrain_bot.Services
{
    public class PermService : IPermService
    {
        private readonly IMongoCollection<PermissionSchema> _Kick;
        private readonly IMongoCollection<PermissionSchema> _Ban;
        private readonly IMongoCollection<PermissionSchema> _Warn;
        public PermService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("connectionStrings")["snipetrain"]);
            var database = client.GetDatabase("userInteractions");

            _Kick = database.GetCollection<PermissionSchema>("kick");
            _Ban = database.GetCollection<PermissionSchema>("ban");
            _Warn = database.GetCollection<PermissionSchema>("warn");

        }
        public async Task AddKickAsync(PermissionSchema model)
        {
            await _Kick.InsertOneAsync(model);
        }
        public async Task AddBanAsync(PermissionSchema model)
        {
            await _Ban.InsertOneAsync(model);
        }
        public async Task AddWarnAsync(PermissionSchema model)
        {
            await _Warn.InsertOneAsync(model);
        }
        public async Task<List<PermissionSchema>> GetDocsAsync()
        {
            return (await _Warn.FindAsync(s => true)).ToList();
        }
        public async Task<long> GetDocsAsync(IGuildUser user)
        {
            return await _Warn.CountDocumentsAsync(x => x.UserId == user.Id);
        }
    }
}
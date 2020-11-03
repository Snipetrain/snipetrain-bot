using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class SnipetrainService : ISnipetrainService
    {

        private RestClient _client;

        public SnipetrainService(IConfiguration configuration)
        {
            _client = new RestClient(configuration.GetSection("endpoints")["snipetrain-api"]);
        }

        public async Task<Pagination<List<Player>>> GetRankAsync(string query, string game, int perPage)
        {
            var request = new RestRequest($"/api/ranks/{game}/{query}?perPage={perPage}");
            return await _client.GetAsync<Pagination<List<Player>>>(request);
        }

        public async Task<Pagination<List<Player>>> GetTop10Async(string game)
        {
            var request = new RestRequest($"/api/ranks/{game}/?perPage=10");
            return await _client.GetAsync<Pagination<List<Player>>>(request);
        }

    }    
}
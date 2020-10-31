using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class HttpClientService
    {

        private RestClient _client;

        public HttpClientService(string baseUri)
        {
            _client = new RestClient(baseUri);
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
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class TwitchService : ITwitchService
    {
        private RestClient _client;
        private readonly string _twitchToken;

        public TwitchService(IConfiguration configuration)
        {
            _client = new RestClient(configuration.GetSection("endpoints")["twitch"]);
            _twitchToken = configuration["twitchToken"];
        }

        public async Task<TwitchUser> GetTwitchUser(string twitchUser)
        {
            var request = new RestRequest($"/users?login={twitchUser}");

            request.AddHeader("Authorization", $"Bearer {_twitchToken}");
            request.AddHeader("Client-Id", "");

            var res = await _client.GetAsync<TwitchWrapper<TwitchUser>>(request);

            return res.Data[0];
        } 
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class TwitchService : ITwitchService
    {
        private readonly RestClient _apiClient;
        private readonly RestClient _authClient;
        private readonly IConfiguration _config;

        private string TwitchToken;

        public TwitchService(IConfiguration configuration)
        {
            _config = configuration;

            _apiClient = new RestClient(_config.GetSection("endpoints")["twitch-api"]);
            _authClient = new RestClient(_config.GetSection("endpoints")["twitch-auth"]);
        }

        public async Task<TwitchUser> GetTwitchUser(string twitchUser)
        {
            var request = new RestRequest($"/users?login={twitchUser}");

            request.AddHeader("Authorization", $"Bearer {TwitchToken}");
            request.AddHeader("Client-Id", _config.GetSection("twitchConfig")["clientId"]);

            var res = await _apiClient.GetAsync<TwitchUserWrapper>(request);

            return res.Data[0];
        }

        public async Task AuthenticateTwitch()
        {
            try
            {
                var clientId = _config.GetSection("twitchConfig")["clientId"];
                var clientSecret = _config.GetSection("twitchConfig")["clientSecret"];

                var request = new RestRequest($"/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials");
                var res = await _authClient.PostAsync<OAuthResponse>(request);

                TwitchToken = res.AccessToken;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to authenticate with Twitch.TV :: {e.ToString()}");
                throw e;
            }
        }
    }
}
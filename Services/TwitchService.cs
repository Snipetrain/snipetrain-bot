using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using snipetrain_bot.Events;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public class TwitchService : ITwitchService
    {
        private readonly RestClient _apiClient;
        private readonly RestClient _authClient;
        private readonly IConfiguration _config;
        private readonly IStreamersService _streamerService;
        private readonly IServiceProvider _serviceProvider;

        private string TwitchToken;

        private List<TwitchSubscription> Subscriptions = new List<TwitchSubscription>(); 

        public TwitchService(IConfiguration configuration, IStreamersService streamersService, IServiceProvider serviceProvider)
        {
            _config = configuration;
            _streamerService = streamersService;
            _serviceProvider = serviceProvider;

            _apiClient = new RestClient(_config.GetSection("twitch").GetSection("endpoints")["twitch-api"]);
            _authClient = new RestClient(_config.GetSection("twitch").GetSection("endpoints")["twitch-auth"]);
        }

        public async Task<TwitchUser> GetTwitchUser(string twitchUser)
        {
            var request = new RestRequest($"/users?login={twitchUser}");

            request.AddHeader("Authorization", $"Bearer {TwitchToken}");
            request.AddHeader("Client-Id", _config.GetSection("twitch")["clientId"]);

            var res = await _apiClient.GetAsync<TwitchUserWrapper>(request);

            return res.Data[0];
        }

        public async Task AuthenticateTwitch()
        {
            try
            {
                var clientId = _config.GetSection("twitch")["clientId"];
                var clientSecret = _config.GetSection("twitch")["clientSecret"];

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
        public async Task AddAllTwitchSubscriptions()
        {
            var streamers = await _streamerService.GetStreamersAsync();

            foreach (var streamer in streamers)
            {
                AddTwitchSubscription(streamer.Name, streamer.Id);
            }
        }
        public void AddTwitchSubscription(string twitchUsername, string twitchUserId)
        {
            var runner = _serviceProvider.GetService(typeof(DiscordRunner)) as DiscordRunner;
            Subscriptions.Add(new TwitchSubscription(runner, _config, twitchUsername, twitchUserId, TwitchToken));
        }

        public void RemoveTwitchSubscription(string twitchUsername)
        {
            var sub = Subscriptions.Find(x => x.Username == twitchUsername);
            sub.Disconnect();
            Subscriptions.Remove(sub);
        }
    }
}
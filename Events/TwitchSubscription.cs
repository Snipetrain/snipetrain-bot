using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace snipetrain_bot.Events
{
    public class TwitchSubscription 
    {
        public string Username;
        private readonly TwitchPubSub _client;
        private readonly string _token;
        private readonly DiscordRunner _runner;
        private readonly IConfiguration _config;
        public TwitchSubscription(DiscordRunner runner, IConfiguration config, string twitchUsername, string twitchUserId, string token)
        {
            Username = twitchUsername;

            _token = token;
            _runner = runner;
            _config = config;

            _client = new TwitchPubSub();

            _client.OnPubSubServiceConnected += onPubSubServiceConnected;
            _client.OnListenResponse += onListenResponse;
            _client.OnStreamUp += onStreamUp;
            
            _client.ListenToVideoPlayback(twitchUsername);
            
            _client.Connect();
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }

        private void onPubSubServiceConnected(object sender, EventArgs e)
        {
            // SendTopics accepts an oauth optionally, which is necessary for some topics
            _client.SendTopics();
        }
        
        private void onListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
                throw new Exception($"Failed to listen! Response: {e.Response}");
        }
        private async void onStreamUp(object sender, OnStreamUpArgs e)
        {
            var streamChannelId = ulong.Parse(_config.GetSection("discord").GetSection("channels")["stream-announcement"]);
            var roleId = _config.GetSection("discord").GetSection("roles").GetSection("stream")["id"];
            await _runner.SendMessage($"[ <@&{roleId}> ] {Username} just went online on Twitch! URL: https://twitch.tv/{Username}", streamChannelId);
        }
    }
}
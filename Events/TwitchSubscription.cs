using System;
using System.Threading.Tasks;
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
        public TwitchSubscription(DiscordRunner runner, string twitchUsername, string twitchUserId, string token)
        {
            Username = twitchUsername;
            _token = token;
            _runner = runner;

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
            await _runner.SendMessage($"{Username} just went online on Twitch! URL: https://twitch.tv/{Username}");
        }
    }
}
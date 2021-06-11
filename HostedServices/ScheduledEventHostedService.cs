using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using snipetrain_bot.Services;

namespace snipetrain_bot.HostedServices
{
    public class ScheduledPartyHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
        private readonly ILogger<ScheduledPartyHostedService> _logger;
        private readonly IPartyService _partyService;
        private readonly DiscordRunner _runner;
        private readonly IConfiguration _config;

        public ScheduledPartyHostedService(IPartyService partyService, DiscordRunner runner,IConfiguration config)
        {
            _partyService = partyService;
            _runner = runner;
            _config = config;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            var seconds = 60;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                var partyList = await _partyService.GetPartiesAsync();
                var ChannelNA = ulong.Parse(_config.GetSection("channels")["snipetrain-tf2-us"]);
                var ChannelEU = ulong.Parse(_config.GetSection("channels")["snipetrain-tf2-eu"]);
                var roleId = _config.GetSection("roles").GetSection("event_mention")["id"];

                foreach (var party in partyList)
                {
                    /*var DateCompVal = party.EventDay.CompareTo(DateTimeOffset.Now);

                    if (DateCompVal <= 0)
                    {
                        if (party.Region == "NA")
                        {
                            await _runner.SendMessage($"<@&{roleId}>  an NA Event is Starting Right Now", ChannelNA);
                        }
                        if (party.Region == "EU")
                        {
                            await _runner.SendMessage($"<@&{roleId}>  an EU Event is Starting Right Now", ChannelEU);
                        }
                        await _partyService.RemovePartyAsync(party.Id);
                    }*/
                }
            }
            catch (System.Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
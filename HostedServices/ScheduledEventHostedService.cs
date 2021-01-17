using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using snipetrain_bot.Models;
using snipetrain_bot.Events;
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

        public ScheduledPartyHostedService(IPartyService partyService, DiscordRunner runner)
        {
            _partyService = partyService;
            _runner = runner;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            var seconds = 60;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var partyList = await _partyService.GetPartiesAsync();
            var ChannelNA = ulong.Parse("700712690288427129");
            var ChannelEU = ulong.Parse("747139803711012884");
            var roleId = "272275923208896512";

            foreach (var party in partyList)
            {
                var DateCompVal = party.EventDay.CompareTo(DateTime.Now);

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
                    }
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
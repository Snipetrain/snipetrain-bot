using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using snipetrain_bot.Services;
using snipetrain_bot.Models;
using System.Linq;

namespace snipetrain_bot.HostedServices
{
    public class ScheduledPartyHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
        private readonly ILogger<ScheduledPartyHostedService> _logger;
        private readonly IPartyService _partyService;
        private readonly DiscordRunner _runner;
        private readonly IConfiguration _config;

        public ScheduledPartyHostedService(IPartyService partyService, DiscordRunner runner, IConfiguration config)
        {
            _partyService = partyService;
            _runner = runner;
            _config = config;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            var seconds = 30;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var votingParties = await _partyService.GetVotingPartyAsync();

            if (votingParties != null)
            {
                if (votingParties.ExpiryDate <= DateTime.UtcNow)
                {
                    await _runner.SendMessage("Expired.", 309459839296208897);
                    await _partyService.UpdatePartyStateAsync(votingParties, Models.PartyState.Inactive);
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
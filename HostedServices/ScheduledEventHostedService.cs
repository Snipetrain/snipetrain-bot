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

    
        public ScheduledPartyHostedService(IPartyService partyService)
        {
            _partyService = partyService;
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

                foreach (var party in partyList)
                {
                    Console.WriteLine($"party = {party.Name}");
                }

            }
            catch (Exception ex)
            {
                
                throw;
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
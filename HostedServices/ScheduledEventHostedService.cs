using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using snipetrain_bot.Models;

namespace snipetrain_bot.HostedServices
{
    public class ScheduledEventHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
        private List<EventSchema> list = new List<EventSchema>();

        public DataUpdateHostedService()
        {
            
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            
            var seconds = 60;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            // DO STUFF EVERY 60 SECONDS
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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using snipetrain_bot.Models;
using snipetrain_bot.Events;
using Microsoft.Extensions.Logging;

namespace snipetrain_bot.HostedServices
{
    public class ScheduledEventHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
        private List<EventsList> Events = new List<EventsList>();
        private readonly ILogger<ScheduledEventHostedService> _logger;
        private readonly DateTime _date;

        public DataUpdateHostedService(Timer timer,DateTime date)
        {
            _timer = timer;
            _date = date;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            
            var seconds = 60;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            
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
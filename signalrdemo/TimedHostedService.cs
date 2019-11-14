using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace signalrdemo
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            this._hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            //_logger.LogInformation("Timed Background Service is working.");

            await _hubContext.Clients.All.SendAsync("ReceiveMessage","datetime", DateTime.Now.ToString());
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

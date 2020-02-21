using Microsoft.Extensions.Hosting;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiCore2
{
    public class HostedService : IHostedService
    {
        private MetricPusher metricPusher;
        public HostedService()
        {
            metricPusher = new MetricPusher(new MetricPusherOptions
            {
                Endpoint = "http://localhost:5000/metrics",
                Job = "dicomstore",
                IntervalMilliseconds = 5000
            });
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            metricPusher.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return metricPusher.StopAsync();
        }
    }
}

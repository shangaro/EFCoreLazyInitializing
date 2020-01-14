using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BirdIsAWord.BusConfiguration
{
    /// <summary>
    /// hosted service for starting and stopping masstransit bus
    /// see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.1 for hosted service
    /// see http://masstransit-project.com/MassTransit/usage/containers/msdi.html for masstransit sample setup
    /// see startup.cs class for dependency injection (bus, consumers, etc.)
    /// </summary>

    public class BusHostedService : IHostedService
    {
        private readonly ILogger<BusHostedService> _logger;
        private readonly IBusControl _busControl;

        public BusHostedService(ILogger<BusHostedService> logger,IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(BusHostedService)}");
            await _busControl.StartAsync(cancellationToken);
            _logger.LogInformation($"{nameof(BusHostedService)} started");

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(BusHostedService)}");
            await _busControl.StopAsync(cancellationToken);
            _logger.LogInformation($"{nameof(BusHostedService)} stopped");

        }
    }
}

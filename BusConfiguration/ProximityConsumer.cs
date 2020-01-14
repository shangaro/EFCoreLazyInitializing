using BirdIsAWord.Data;
using MassTransit;
using System.Threading.Tasks;
using BirdIsAWord.Data.Entities;
using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BirdIsAWord.BusConfiguration
{
    public class ProximityConsumer : IConsumer<ProximityCommand>
    {
        private readonly IRepositoryAsync<Proximity> _efRepository;
        private readonly ILogger<ProximityConsumer> _logger;
        private bool success = false;
        public ProximityConsumer(IRepositoryAsync<Proximity> efRepository,ILogger<ProximityConsumer> logger)
        {
            _efRepository = efRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ProximityCommand> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var data = await _efRepository.ListAllAsync();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            if (data != null)
            {
                _logger.LogInformation($"data has been read successfully in time {elapsedTime}");
            }
            
        }
    }
}
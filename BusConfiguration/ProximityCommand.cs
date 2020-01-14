using System;

namespace BirdIsAWord.BusConfiguration
{
    public class ProximityCommand
    {
        public ProximityCommand()
        {
            this.CorrelationId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.UtcNow.ToLongDateString();
        }
        public string CorrelationId { get; set; }
        public string TimeStamp { get; set; }
    }
}
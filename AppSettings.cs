using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirdIsAWord
{
    public class AppSettings
    {
        public MassTransitConfigurationOptions RabbitMQ { get; set; }
    }

    public class MassTransitConfigurationOptions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>")]
        public string HostUri { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool UseInMemoryTransport { get; set; }

    }
}

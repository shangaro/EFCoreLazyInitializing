using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using Microsoft.Extensions.DependencyInjection;



namespace BirdIsAWord.BusConfiguration
{
    internal static class MassTransitConfiguration
    {
        /// <summary>
        /// Configure the application to use masstransit. This cofigures ths host (RabbitMq)
        /// And sets up consumers, receive endpoints, etc.
        /// </summary>
        /// <param name="services">
        /// The Microsoft.Extensions.DependencyInjection.IServiceCollection
        /// to add the services to.
        /// </param>
        /// <returns>
        /// The Microsoft.Extensions.DependencyInjection.IServiceCollection
        /// so that additional calls can be chained.
        /// </returns>
        public static IServiceCollection SetupMassTransitWithRabbitMq(this IServiceCollection services)
        {
           

            services.AddScoped(typeof(ProximityConsumer));
            services.AddScoped(typeof(ProximityCommand));
            services.AddMassTransit(
                provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.ConfigureJsonSerializer(x =>
                    {
                        // non-decimal numbers are read as Int64 by default.
                        x.FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Decimal;
                        return x;
                    });

                    var rabbitMQOptions = provider.GetService<AppSettings>().RabbitMQ;
                    if (rabbitMQOptions.UseInMemoryTransport)
                    {
                        throw new NotImplementedException("InMemory transport configuration is not setup yet.");
                    }

                    
                    var host = cfg.Host(new Uri(rabbitMQOptions.HostUri), hostConfigurator =>
                    {
                        if (!string.IsNullOrEmpty(rabbitMQOptions.Username))
                        {
                            hostConfigurator.Username(rabbitMQOptions.Username);
                        }

                        if (!string.IsNullOrEmpty(rabbitMQOptions.Password))
                        {
                            hostConfigurator.Password(rabbitMQOptions.Password);
                        }
                    });

                    

                    /*
                     * to configure propertly, each event handler should have it's own queue,
                     * so several read model genreators can receive copies of the same message
                     * All commands should be processed by the same queue, so each command is processed only once.
                     */
                    cfg.ReceiveEndpoint("mdm-data-commands", e =>
                    {
                        e.ConfigureConsumer<ProximityConsumer>(provider);
                        

                        // setup a convention to let masstransit know all these commands should be sent
                        // to this queue.
                        EndpointConvention.Map<ProximityCommand>(e.InputAddress);

                    });

                    

                }),
                x =>
                {
                    // command handlers
                    x.AddConsumer<ProximityConsumer>();


                    //// event handlers
                    //x.AddConsumer<ProximityConsumer>();

                    // request client
                    x.AddRequestClient<ProximityCommand>();
                    
                   
                });
            services.AddMassTransitHostedService();
            //services.AddHostedService<BusHostedService>();

            return services;
        }
    }
}



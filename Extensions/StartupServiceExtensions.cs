using BirdIsAWord.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirdIsAWord.BusConfiguration;

namespace BirdIsAWord.Extensions
{
    public static class StartupServiceExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(EFRepository<>));
            //services.AddScoped(typeof(EFRepository<>), typeof(IRepository<>));
            services.SetupMassTransitWithRabbitMq();
        }
    }
}

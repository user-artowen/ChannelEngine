using System;
using System.IO;
using System.Threading.Tasks;
using ChannelEngineCommonData.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChannelEngineDemoConsole.Classes
{
    public static class ServiceHelper
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IOrderService, OrderService>();
                });

            return hostBuilder;
        }
    }
}

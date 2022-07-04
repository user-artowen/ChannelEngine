using System;
using System.Threading.Tasks;
using ChannelEngineCommonData.Service;
using ChannelEngineDemoConsole.Classes;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        int topX = 5;
        
        var host = ServiceHelper.CreateHostBuilder(args).Build();
        var orderService = host.Services.GetService<IOrderService>();
        await ConsoleHelper.DisplayProductTop(orderService, topX);
               
        Console.Read();
    }
}
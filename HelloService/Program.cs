using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloService.Contracts.Events;
using MassTransit;

namespace HelloService
{
    class Program
    {
        const string ServiceBusHost = "{ServiceBusConnectionString}";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg => cfg.Host(ServiceBusHost));

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            int counter = 0;

            await busControl.StartAsync(source.Token);
            try
            {
                while (true)
                {
                    var (command, value) = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        var line = Console.ReadLine() + " padding";
                        var input = line.Split(' ');
                        return (input[0], input[1]);
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    var tasks = new List<Task>();
                    foreach (var item in Enumerable.Range(1, 10))
                    {
                        if ("send".Equals(command, StringComparison.OrdinalIgnoreCase))
                        {
                            var sendEndpoint = await busControl.GetSendEndpoint(new Uri("queue:event-listener"));
                            tasks.Add(sendEndpoint.Send<DocumentOperationCompleted>(new
                            {
                                CompleteValue = counter++,
                                OperationCompletedData = new
                                {
                                    OperationOutputData = value
                                }
                            }));
                        }
                        else
                            tasks.Add(busControl.Publish<DocumentOperationCompleted>(new
                            {
                                CompleteValue = counter++,
                                OperationCompletedData = new
                                {
                                    OperationOutputData = command
                                }
                            }));
                    }

                    Console.WriteLine("All 10 tasks out");
                    await Task.WhenAll(tasks);
                    Console.WriteLine("tasks done");
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}

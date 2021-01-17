using System;
using System.Threading;
using System.Threading.Tasks;
using HelloService.Contracts.Events;
using MassTransit;

namespace GoodbyeService
{
    class Program
    {
        const string ServiceBusHost = "{ServiceBusConnectionString}";
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingAzureServiceBus(serviceBus =>
            {
                serviceBus.Host(ServiceBusHost);
                serviceBus.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<DocumentOperationCompletedConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        
    }
    class DocumentOperationCompletedConsumer : IConsumer<DocumentOperationCompleted>
    {
        public Task Consume(ConsumeContext<DocumentOperationCompleted> context)
        {
            Console.WriteLine($"ThreadId {Environment.CurrentManagedThreadId}: Consumer:{GetType().Name} handles Value:{context.Message.CompleteValue} Text:{context.Message.OperationCompletedData.OperationOutputData}");
            return Task.CompletedTask;
        }
    }
}

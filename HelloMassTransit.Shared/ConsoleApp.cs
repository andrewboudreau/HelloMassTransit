using System;

namespace HelloMassTransit.Shared
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using HelloService.Contracts.Events;
    using MassTransit;
    using Microsoft.Extensions.Configuration;

    namespace GoodbyeService
    {
        class Program
        {
            public static IConfigurationRoot Configuration { get; private set; }

            public static string ServiveBusConnectionString { get; private set; }

            public static async Task Main()
            {
                Configuration = Configure();
                var busControl = Bus.Factory.CreateUsingAzureServiceBus(serviceBus =>
                {
                    serviceBus.Host(ServiveBusConnectionString);
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


            protected virtual static IConfigurationRoot Configure()
            {
                var configurationBuilder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddUserSecrets<Program>();

                Configuration = configurationBuilder.Build();
                ServiveBusConnectionString = Configuration["ServiceBusConnection"];

                return Configuration;
            }
        }
    }
}

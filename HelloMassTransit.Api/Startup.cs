using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloMassTransit.Api.Consumers;
using HelloService.Contracts.Events;
using HelloService.Contracts.Requests;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloMassTransit.Api
{
    //https://daveabrock.com/2020/12/04/migrate-mvc-to-route-to-code
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services
                .AddMassTransit(cfg =>
                {
                    cfg.AddConsumersFromNamespaceContaining<DocumentOperationCompletedConsumer>();
                    cfg.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(Configuration["ServiceBusConnection"]);
                        cfg.ConfigureEndpoints(context);
                    });
                })
                .AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapGet("/api/hello/{id:int}/{color}", async context =>
                {
                    var bus = context.RequestServices.GetRequiredService<IBus>();

                    var id = int.Parse(context.Request.RouteValues["id"].ToString());
                    var color = context.Request.RouteValues["color"].ToString();

                    await bus.Publish<DocumentOperationCommand>(new
                    {
                        DocumentIdentifier = id,
                        Settings = new
                        {
                            AnotherBooleanValue = true,
                            BackgroundColors = color
                        }
                    });
                });

                endpoints.MapGet("/api/hello/complete/{id:int}/{data}", async context =>
                {
                    var bus = context.RequestServices.GetRequiredService<IBus>();

                    var id = int.Parse(context.Request.RouteValues["id"].ToString());
                    var data = context.Request.RouteValues["data"].ToString();

                    await bus.Publish<DocumentOperationCompleted>(new
                    {
                        CompleteValue = id,
                        OperationCompletedData = new
                        {
                            OperationOutputData = data
                        }
                    });
                });
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using Library.NotificationService2.Configuration;
using Library.WebApi;
using Library.WebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Library.NotificationService2
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                var rabbitConfiguration = Configuration.GetSection("RabbitMQ").Get<RabbitMqConfiguration>();

                x.AddConsumer<RentedBookConsumer>();
                
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(rabbitConfiguration.ServerAddress), hostConfigurator => 
                    { 
                        hostConfigurator.Username(rabbitConfiguration.Username);
                        hostConfigurator.Password(rabbitConfiguration.Password);
                    });

                    cfg.ReceiveEndpoint(host, "notification-service", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<RentedBookConsumer>(provider);
                    });
                }));
            });
            
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            
            services.AddSingleton<IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) => { await context.Response.WriteAsync("Notification Service started!"); });
        }
    }

    public class RentedBookConsumer : IConsumer<BookRented>
    {
        public Task Consume(ConsumeContext<BookRented> context)
        {
            Console.WriteLine($"The {context.Message.Title} with id: {context.Message.Id} was rented");
            return Task.CompletedTask;
        }
    }
}
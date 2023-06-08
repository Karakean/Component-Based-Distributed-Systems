using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Warehouse
{
    class Program
    {
        static int free = 0;
        static int reserved = 0;

        public class HandlerClass : IConsumer<ConfirmationRequest>, IConsumer<ShopResponse>
        {
            public Task Consume(ConsumeContext<ConfirmationRequest> ctx)
            {
                Console.WriteLine($"Order confirmation request received: amount={ctx.Message.amount} name={ctx.Message.name}");
                if (ctx.Message.amount > free)
                {
                    ctx.RespondAsync(new WarehouseConfirmationResponseReject() { CorrelationId = ctx.Message.CorrelationId });
                }
                else
                {
                    free -= ctx.Message.amount;
                    reserved += ctx.Message.amount;
                    ctx.RespondAsync(new WarehouseConfirmationResponseAccept() { CorrelationId = ctx.Message.CorrelationId });
                }
                Console.WriteLine($"Current inventory: {free} free, {reserved} reserved.");
                return Task.CompletedTask;
            }

            public Task Consume(ConsumeContext<ShopResponse> ctx)
            {
                string msg = ctx.Message.text;
                if (!msg.Equals("rejected by the warehouse"))
                {
                    reserved -= ctx.Message.amount;
                    if (!msg.Equals("success"))
                    {
                        free += ctx.Message.amount;
                    }
                }
                Console.WriteLine($"Shop response: {msg}. Current inventory: {free} free, {reserved} reserved.");
                return Task.CompletedTask;
            }
        }

        static void Main(string[] args)
        {
            var consumer = new HandlerClass();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("warehousequeue", ep =>
                {
                    ep.Instance(consumer);
                });
            });
            bus.Start();
            Console.WriteLine("Warehouse started.");
            while (true)
            {
                Console.WriteLine("Enter the amount which you would like to add to the warehouse:");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                int number = 0;
                if (int.TryParse(input, out number))
                {
                    free += number;
                    Console.WriteLine($"You've added {number} piece(s) to the warehouse, now there are {free} piece(s) free to take, {reserved} reserved.");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            Console.ReadKey();
            bus.Stop();
        }
    }
}
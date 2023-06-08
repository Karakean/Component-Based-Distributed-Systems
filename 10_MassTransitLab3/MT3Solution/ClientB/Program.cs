using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace ClientB
{
    class Program
    {
        public class HandlerClass : IConsumer<ConfirmationRequest>, IConsumer<ShopResponse>
        {
            public Task Consume(ConsumeContext<ConfirmationRequest> ctx)
            {
                while (true)
                {
                    Console.WriteLine($"Order confirmation request received: amount={ctx.Message.amount} name={ctx.Message.name}");
                    Console.WriteLine("Do you confirm the order? (y/n)");
                    string input = Console.ReadLine();
                    if (input.Contains("y"))
                    {
                        ctx.RespondAsync(new ClientConfirmationResponseAccept() { CorrelationId = ctx.Message.CorrelationId });
                        return Console.Out.WriteLineAsync($"Confired order: amount={ctx.Message.amount} name={ctx.Message.name}");
                    }
                    else if (input.Contains("n"))
                    {
                        ctx.RespondAsync(new ClientConfirmationResponseReject() { CorrelationId = ctx.Message.CorrelationId });
                        return Console.Out.WriteLineAsync($"Rejected order: amount={ctx.Message.amount} name={ctx.Message.name}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }
                }
            }

            public Task Consume(ConsumeContext<ShopResponse> ctx)
            {
                return Console.Out.WriteLineAsync($"Shop response: {ctx.Message.text}");
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
                sbc.ReceiveEndpoint("clientbrecvqueue", ep =>
                {
                    ep.Instance(consumer);
                });
            });
            bus.Start();
            Console.WriteLine("Enter your name:");
            string myName = Console.ReadLine();
            Console.WriteLine($"{myName} started.");

            int number = 0;
            bool isNumber = false;
            while (!isNumber)
            {
                Console.WriteLine("Enter the amount of product you would like to order:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {
                    isNumber = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            bus.Publish(new Order() { amount = number, name = myName });
            Console.ReadKey();
            Console.ReadKey();
            bus.Stop();
        }
    }
}

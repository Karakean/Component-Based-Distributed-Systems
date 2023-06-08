using MassTransit;
using Messages;
using System;
using System.Threading.Tasks;

namespace ReceiverC
{
    class Program
    {
        public static Task Handler(ConsumeContext<IMessage2> ctx) => Console.Out.WriteLineAsync($"SubscriberC received: {ctx.Message.text2}");

        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueueC", ep =>
                {
                    ep.Handler<IMessage2>(Handler);
                });
            });
            bus.Start();
            Console.WriteLine("SubscriberC started");
            Console.ReadKey();
            bus.Stop();
        }
    }
}

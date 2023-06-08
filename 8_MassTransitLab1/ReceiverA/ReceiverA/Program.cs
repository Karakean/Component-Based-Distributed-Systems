using MassTransit;
using Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReceiverA
{
    class Program
    {
        public static Task Handle(ConsumeContext<IMessage1> ctx) =>
            Console.Out.WriteLineAsync($"SubscriberA received: {ctx.Message.text1}. First header: {ctx.Headers.GetAll().First(elem => elem.Key == "key1").Value}. Second header: {ctx.Headers.GetAll().First(elem => elem.Key == "key2").Value}");

        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueueA", ep =>
                {
                    ep.Handler<IMessage1>(Handle);
                });
            });
            bus.Start();
            Console.WriteLine("SubscriberA started");
            Console.ReadKey();
            bus.Stop();
        }
    }
}

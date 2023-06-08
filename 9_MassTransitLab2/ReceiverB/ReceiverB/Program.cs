using MassTransit;
using Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriberB
{
    public class Publ : IPubl
    {
        public int number { get; set; }
    }

    public class OdpB : IOdpB
    {
        public string kto { get; set; }
    }
    class Program
    {
        public class Consumer : IConsumer<IPubl>, IConsumer<Fault<IOdpB>>
        {
            public Task Consume(ConsumeContext<IPubl> ctx)
            {
                bool replied = false;
                if (ctx.Message.number % 2 == 0)
                {
                    ctx.RespondAsync(new OdpB() { kto = "abonent B" });
                    replied = true;
                }
                string infix = replied ? "" : " not";
                return Console.Out.WriteLineAsync($"SubscriberB received: {ctx.Message.number} and did{infix} reply.");
            }

            public Task Consume(ConsumeContext<Fault<IOdpB>> ctx)
            {
                string buffer = "";
                foreach (var e in ctx.Message.Exceptions)
                {
                    buffer += e.Message;
                }
                return Console.Out.WriteLineAsync(buffer);
            }
        }


        static void Main(string[] args)
        {
            Consumer consumer = new Consumer();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueueb", ep =>
                {
                    ep.Instance(consumer);
                });
            });
            bus.Start();
            Console.WriteLine("SubscriberB started. Press any key to exit.");
            Console.ReadKey();
            bus.Stop();
        }
    }
}

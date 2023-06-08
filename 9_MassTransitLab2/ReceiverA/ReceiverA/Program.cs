using MassTransit;
using Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriberA
{
    public class Publ : IPubl
    {
        public int number { get; set; }
    }

    public class OdpA : IOdpA
    {
        public string kto { get; set; }
    }

    public class OdpB : IOdpB
    {
        public string kto { get; set; }
    }
    class Program
    {
        public class Consumer : IConsumer<IPubl>, IConsumer<Fault<IOdpA>>
        {
            public Task Consume(ConsumeContext<IPubl> ctx)
            {
                bool replied = false;
                if (ctx.Message.number % 2 == 0)
                {
                    ctx.RespondAsync(new OdpA() { kto = "abonent A" });
                    replied = true;
                }
                string infix = replied ? "" : " not";
                return Console.Out.WriteLineAsync($"SubscriberA received: {ctx.Message.number} and did{infix} reply.");
            }

            public Task Consume(ConsumeContext<Fault<IOdpA>> ctx)
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
            var consumer = new Consumer();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueuea", ep =>
                {
                    ep.Instance(consumer);
                });
            });
            bus.Start();
            Console.WriteLine("SubscriberA started. Press any key to exit.");
            Console.ReadKey();
            bus.Stop();
        }
    }
}

using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReceiverB
{
    class HandlerB : IConsumer<IMessage3>
    {
        int counter = 0;

        public Task Consume(ConsumeContext<IMessage3> ctx)
        {
            counter++;
            return Console.Out.WriteLineAsync($"SubscriberB received: {ctx.Message.text1} {ctx.Message.text2}. First header: {ctx.Headers.GetAll().First(elem => elem.Key == "3key1").Value}. Second header: {ctx.Headers.GetAll().First(elem => elem.Key == "3key2").Value}. Counter: {counter}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var handler = new HandlerB();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueueB", ep =>
                {
                    ep.Consumer(typeof(HandlerB), x => handler);
                });
            });
            bus.Start();
            Console.WriteLine("SubscriberB started");
            Console.ReadLine();
            bus.Stop();
        }
    }
}

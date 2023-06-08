using MassTransit;
using Messages;
using System;

namespace Sender
{
    public class Message1 : IMessage1
    {
        public string text1 { get; set; }
    }

    public class Message2 : IMessage2
    {
        public string text2 { get; set; }
    }

    public class Message3 : IMessage3
    {
        public string text1 { get; set; }
        public string text2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
            bus.Start();
            Console.WriteLine("Publisher started");
            for (int i = 0; i < 10; i++)
            {
                String textToSend1 = $"Message{i}";
                bus.Publish(new Message1() { text1 = textToSend1 }, ctx =>
                {
                    ctx.Headers.Set("key1", "value1");
                    ctx.Headers.Set("key2", "value2");
                }
                );
                String textToSend2 = $"MessageSecondType{i}";
                bus.Publish(new Message2() { text2 = textToSend2 });
                String textToSend3 = $"MessageThirdType{i}";
                bus.Publish(new Message3() { text1 = "1" + textToSend3, text2 = "2" + textToSend3 }, ctx =>
                {
                    ctx.Headers.Set("3key1", "3value1");
                    ctx.Headers.Set("3key2", "3value2");
                }
                );
                Console.WriteLine("Sent: " + textToSend1 + " and " + textToSend2 + " and " + "1" + textToSend3 + " 2" + textToSend3);
                Console.ReadKey();
            }
            Console.ReadKey();
            bus.Stop();
        }
    }
}

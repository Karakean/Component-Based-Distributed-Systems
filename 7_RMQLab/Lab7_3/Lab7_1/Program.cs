using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Lab7_3
{
    class MyConsumer : DefaultBasicConsumer
    {
        public MyConsumer(IModel model) : base(model) { }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(message); 
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender");
            Console.ReadKey();

            var factory = new ConnectionFactory()
            {
                UserName = "dcxhdwed",
                Password = "qdjNkU1WL4p2U8VCaV8aH3l0qNs2soVB",
                HostName = "sparrow.rmq.cloudamqp.com",
                VirtualHost = "dcxhdwed"
            };

            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    string queueName = "zad7"; //"zad3"
            //    channel.QueuePurge(queueName);
            //    channel.QueueDeclare(queueName, false, false, false, null);

            //    //Zad6
            //    string replyQueue = channel.QueueDeclare().QueueName;
            //    var consumer = new MyConsumer(channel);
            //    channel.BasicConsume(replyQueue, true, consumer);

            //    for (int i = 0; i < 10; i++)
            //    {
            //        string message = "message number " + i;
            //        var body = Encoding.UTF8.GetBytes(message);
            //        IBasicProperties prop = channel.CreateBasicProperties();
            //        prop.Headers = new Dictionary<string, object>();
            //        prop.Headers.Add("key1", "value1");
            //        prop.Headers.Add("key2", "value2");

            //        //Zad6
            //        prop.ReplyTo = replyQueue;
            //        var correlationId = Guid.NewGuid().ToString();
            //        prop.CorrelationId = correlationId;

            //        channel.BasicPublish("", queueName, prop, body);
            //    }
            //    Console.ReadKey();
            //}
            Console.WriteLine("Zad 7");
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("abc", "topic");

                for (int i = 0; i < 10; i++)
                {
                    var body = Encoding.UTF8.GetBytes("zad7 " + i);

                    if (i % 2 != 0)
                    {
                        channel.BasicPublish("abc", "abc.def", null, body);
                    }
                    else
                    {
                        channel.BasicPublish("abc", "abc.xyz", null, body);
                    }
                }

                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}

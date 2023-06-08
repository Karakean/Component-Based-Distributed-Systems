using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Lab7_1
{
    class MyConsumer : DefaultBasicConsumer
    {
        public MyConsumer(IModel model) : base(model) { }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            // show message
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

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queueName = "zad1";
                channel.QueueDeclare(queueName, false, false, false, null);
                for (int i = 0; i < 10; i++)
                {
                    string message = "message number " + i;
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", queueName, null, body);
                }
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}

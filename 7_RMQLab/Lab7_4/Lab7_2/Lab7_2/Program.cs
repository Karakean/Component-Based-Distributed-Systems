using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client;

namespace Lab7_4
{
    class MyConsumer : DefaultBasicConsumer
    {
        public MyConsumer(IModel model) : base(model) { }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties,
        ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(message); // show message

            //Zad 3
            if (properties.Headers != null)
            {
                if (properties.Headers.ContainsKey("key1"))
                {
                    var header1 = Encoding.UTF8.GetString((byte[])properties.Headers["key1"]);
                    Console.WriteLine("first header " + header1);
                }
                if (properties.Headers.ContainsKey("key2"))
                {
                    var header2 = Encoding.UTF8.GetString((byte[])properties.Headers["key2"]);
                    Console.WriteLine("second header is " + header2);
                }
            }

            //zad6
            if (properties.CorrelationId != null)
            {
                var response = Encoding.UTF8.GetBytes("ACK " + message);
                var replyProp = Model.CreateBasicProperties();
                replyProp.CorrelationId = properties.CorrelationId;
                Model.BasicPublish("", properties.ReplyTo, replyProp, response);
            }

            //zad5
            Thread.Sleep(2000);
            Model.BasicAck(deliveryTag, false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver2");
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
            //    string queueName = "zad7"; //"zad3"; //string queueName = "zad1";
            //    channel.QueueDeclare(queueName, false, false, false, null);
            //    var consumer = new MyConsumer(channel);
            //    channel.BasicQos(0, 1, false);
            //    channel.BasicConsume(queueName, false, consumer);
            //    Console.ReadKey();
            //}
            //Console.ReadKey();
            Console.WriteLine("Zad7");
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("abc", "topic");
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, "abc", "abc.#");
                var consumer = new MyConsumer(channel);
                channel.BasicConsume(queueName, false, consumer);
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}

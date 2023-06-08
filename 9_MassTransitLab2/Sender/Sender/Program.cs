using MassTransit;
using MassTransit.Events;
using MassTransit.Serialization;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{

    public class Klucz : SymmetricKey
    {
        public byte[] IV { get; set; }
        public byte[] Key { get; set; }
    }
    public class Dostawca : ISymmetricKeyProvider
    {
        private string k; 
        public Dostawca(string _k) 
        { k = _k; }
        public bool TryGetKey(string keyId, out SymmetricKey key)
        {
            var sk = new Klucz(); 
            sk.IV =
            Encoding.ASCII.GetBytes(keyId.Substring(0, 16)); 
            sk.Key = Encoding.ASCII.GetBytes(k); key = sk; 
            return true;
        }
    }
    public class Publ : IPubl
    {
        public int number { get; set; }
    }

    public class Ustaw : IUstaw
    {
        public bool dziala { get; set; }
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
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        static bool generatorWorking = false;
        public static Task ControllerHandler(ConsumeContext<IUstaw> ctx)
        {
            string infix = generatorWorking == ctx.Message.dziala ? "already" : "";
            string postfix = ctx.Message.dziala ? "on" : "off";
            generatorWorking = ctx.Message.dziala;
            return Console.Out.WriteLineAsync($"Generator {infix} turned {postfix}");
        }

        public class AnswerAHandler : IConsumer<IOdpA>
        {
            int retryNum = 0;
            public Task Consume(ConsumeContext<IOdpA> ctx)
            {
                return Task.Run(() => {
                    Random random = new Random();
                    if (random.Next(3) == 1)
                    {
                        throw new Exception($"Exception occured during handling answer from SubscriberA (OdpA) in Publisher. Tried {++retryNum} times");
                    }
                    string postfix = retryNum == 0 ? "" : $"Exception occured {retryNum} time(s) during handling.";
                    Console.Out.WriteLineAsync($"AnswerA: {ctx.Message.kto} replied. {postfix}");
                });
               
            }
        }


        public class AnswerBHandler : IConsumer<IOdpB>
        {
            int retryNum = 0;
            public Task Consume(ConsumeContext<IOdpB> ctx)
            {
                return Task.Run(() =>
                {
                    //Random random = new Random();
                    //if (random.Next(3) == 0)
                    //{
                        throw new Exception($"Exception occured during handling answer from SubscriberB (OdpB) in Publisher. Tried {++retryNum} times");
                    //}
                    string postfix = retryNum == 0 ? "" : $"Exception occured {retryNum} time(s) during handling.";
                    Console.Out.WriteLineAsync($"AnswerB: {ctx.Message.kto} replied. {postfix}");
                });
            }
        }

        class PublishObserver : IPublishObserver
        {
            public int counterPubl = 0;
            public int counterPrePublish = 0;
            public int counterPublishFault = 0;
            public Task PostPublish<T>(PublishContext<T> context) where T : class
            {
                return Task.Run(() => { counterPubl++; });
            }

            public Task PrePublish<T>(PublishContext<T> context) where T : class
            {
                return Task.Run(() => { counterPrePublish++; });
            }

            public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
            {
                return Task.Run(() => { counterPublishFault++; });
            }
        }

        class RecvObserver : IReceiveObserver
        {
            public int counterConsumeFault = 0;
            public int counterPostConsume = 0;
            public int counterPreRecv = 0;
            public int counterPostRecv = 0;
            public int counterRecvFault = 0;

            public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
            {
                return Task.Run(() => { counterConsumeFault++; });
            }

            public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
            {
                return Task.Run(() => { counterPostConsume++; });
            }

            public Task PostReceive(ReceiveContext context)
            {
                return Task.Run(() => { counterPostRecv ++; });
            }

            public Task PreReceive(ReceiveContext context)
            {
                return Task.Run(() => { counterPreRecv ++; });
            }

            public Task ReceiveFault(ReceiveContext context, Exception exception)
            {
                return Task.Run(() => { counterRecvFault++;  });
            }
        }

        static void Main(string[] args)
        {
            const int MAX_RETRY_NUM = 5;
            var pubBus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.UseEncryptedSerializer(new AesCryptoStreamProvider(new Dostawca("18486518486518486518486518486518"), "1848651848651848"));
                sbc.ReceiveEndpoint("recvqueuepub", ep =>
                {
                    ep.Handler<IUstaw>(ControllerHandler);
                });
            });
            RecvObserver recvObserver = new RecvObserver();
            pubBus.ConnectReceiveObserver(recvObserver);
            pubBus.Start();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                sbc.ReceiveEndpoint("recvqueueansa", ep =>
                {
                    ep.Consumer<AnswerAHandler>(cfg =>
                    {
                        cfg.UseRetry(r =>
                        { r.Immediate(MAX_RETRY_NUM); });
                    });
                });
                sbc.ReceiveEndpoint("recvqueueansb", ep =>
                {
                    ep.Consumer<AnswerBHandler>(cfg =>
                    {
                        cfg.UseRetry(r =>
                        { r.Immediate(MAX_RETRY_NUM); });
                    });
                });
            });
            PublishObserver publishObserver = new PublishObserver();
            bus.ConnectPublishObserver(publishObserver);
            bus.ConnectReceiveObserver(recvObserver);
            bus.Start();
            Console.WriteLine("Publisher started. Press ESC to exit");

            bool exit = false;
            Thread inputThread = new Thread(() => {
                while (!exit)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        exit = true;
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.S)
                    {
                        Console.WriteLine("Statistics:");
                        Console.WriteLine($"Publishing stats: {publishObserver.counterPrePublish} pre-published, {publishObserver.counterPubl} published, {publishObserver.counterPublishFault} failed");
                        Console.WriteLine($"Receiving stats: {recvObserver.counterPreRecv} pre-received, {recvObserver.counterPostRecv} post-received, {recvObserver.counterConsumeFault} failed");

                    }
                }
            });
            inputThread.Start();
            int i = 1;
            while (!exit)
            {
                while (generatorWorking)
                {
                    bus.Publish(new Publ() { number = i });
                    Console.WriteLine($"Publisher published: {i}");
                    Thread.Sleep(1000);
                    i++;
                }
            }
            bus.Stop();
            Console.WriteLine("Publisher finished");
        }
    }
}

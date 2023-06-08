
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Serialization;
using Messages;

namespace Controller
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
    public class Ustaw : IUstaw
    {
        public bool dziala { get; set; }
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
                sbc.UseEncryptedSerializer(new AesCryptoStreamProvider(new Dostawca("18486518486518486518486518486518"), "1848651848651848"));
            });
            bus.Start();
            Console.WriteLine("Controller started. Press S to start generator, T to stop it or ESC to leave.");

            bool controllerWorking = true;
            while (controllerWorking)
            {
                var pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Escape:
                        controllerWorking = false;
                        break;
                    case ConsoleKey.S:
                        bus.Publish(new Ustaw() { dziala = true });
                        break;
                    case ConsoleKey.T:
                        bus.Publish(new Ustaw() { dziala = false });
                        break;
                    default:
                        Console.WriteLine("It's not a valid key. Press S to start generator, T to stop it or ESC to leave.");
                        break;
                }
            }
            bus.Stop();
            Console.WriteLine("Controller finished");
        }
    }
}

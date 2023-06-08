using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lab6_6_1
{
    [ServiceContract]
    public interface IZadanie6
    {
        [OperationContract]
        int Dodaj(int a, int b);
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IZadanie6>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/router"));
            var client = factory.CreateChannel();
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Zadanie 6 dziala. 400 + 20 = " + client.Dodaj(400, 20));
            }
            ((IDisposable)client).Dispose();
            factory.Close();
            Console.ReadKey();
        }
    }
}

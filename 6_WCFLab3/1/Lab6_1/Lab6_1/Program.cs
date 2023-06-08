using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace Lab6_1
{
    [ServiceContract]
    public interface IZadanie1
    {
        [OperationContract]
        string ScalNapisy(string a, string b);
    }

    public class Zadanie1 : IZadanie1
    {
        public string ScalNapisy(string a, string b)
        {
            return a + b;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie1));
            serviceHost.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint("soap.udp://localhost:30703"));
            serviceHost.AddServiceEndpoint(typeof(IZadanie1), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf3");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}

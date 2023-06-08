using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace Lab6_2
{
    [ServiceContract]
    public interface IZadanie1
    {
        [OperationContract]
        string ScalNapisy(string a, string b);
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint("soap.udp://localhost:30703"));
            System.Collections.ObjectModel.Collection<EndpointDiscoveryMetadata> endpoints = discoveryClient.Find(new FindCriteria(typeof(IZadanie1))).Endpoints;
            discoveryClient.Close();
            if (endpoints.Count <= 0)
            {
                Console.WriteLine("Brak odpowiednich endpointow.");
            } 
            else {
                var address = endpoints[0].Address;
                var channel = ChannelFactory<IZadanie1>.CreateChannel(new NetNamedPipeBinding(), address);
                Console.WriteLine(channel.ScalNapisy("Zadanie 1 oraz Zadanie 2 ", "dzialaja :)"));
                Console.ReadKey();
                ((IDisposable)channel).Dispose();
            }

        }
    }
}

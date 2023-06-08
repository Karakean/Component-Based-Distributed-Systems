using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Routing;
using System.ServiceModel.Dispatcher;

namespace Router
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(RoutingService));
            serviceHost.AddServiceEndpoint(typeof(IRequestReplyRouter), new NetNamedPipeBinding(), "net.pipe://localhost/router");
            RoutingConfiguration routingConfiguration = new RoutingConfiguration();
            var contract = ContractDescription.GetContract(typeof(IRequestReplyRouter));
            var service1 = new ServiceEndpoint(contract, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/service1"));
            var service2 = new ServiceEndpoint(contract, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/service2"));
            var endpoints = new List<ServiceEndpoint>();
            endpoints.Add(service1);
            endpoints.Add(service2);
            routingConfiguration.FilterTable.Add(new MatchAllMessageFilter(), endpoints);
            serviceHost.Description.Behaviors.Add(new RoutingBehavior(routingConfiguration));
            serviceHost.Open();
            Console.WriteLine("Router dziala :)");
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}

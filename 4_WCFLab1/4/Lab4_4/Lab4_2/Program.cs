using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using KSR_WCF1;

[ServiceContract]
public interface IZadanie2
{
    [OperationContract] string Test(string arg);
}

public class Zadanie2 : IZadanie2
{
    public string Test(string arg)
    {
        return $"Zadanie 4 dziala. Podany argument: {arg}";
    }
}

namespace Lab4_4
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie2));
            serviceHost.AddServiceEndpoint(typeof(IZadanie2), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad2");

            var serviceMetadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (serviceMetadataBehavior == null) {
                serviceMetadataBehavior = new ServiceMetadataBehavior();
            }
            serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexNamedPipeBinding(), "net.pipe://localhost/metadata");

            serviceHost.AddServiceEndpoint(typeof(IZadanie2), new NetTcpBinding(), "net.tcp://127.0.0.1:55765");

            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
            Console.ReadLine();
        }
    }
}

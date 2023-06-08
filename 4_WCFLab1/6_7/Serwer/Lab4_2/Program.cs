using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using KSR_WCF1;

[ServiceContract]
public interface IZadanie7
{
    [OperationContract] [FaultContract(typeof(Wyjatek7))] void RzucWyjatek7(string a, int b);
}


[DataContract]
public class Wyjatek7
{
    [DataMember] public string opis { get; set; }
    [DataMember] public string a { get; set; }
    [DataMember] public int b { get; set; }
}

public class Zadanie7 : IZadanie7
{
    void IZadanie7.RzucWyjatek7(string a, int b)
    {
        FaultException<Wyjatek7> e = new FaultException<Wyjatek7>(new Wyjatek7(), new FaultReason($"Wyjatek! a: {a}, b: {b}"));
        throw e;
    }
}

namespace Lab4_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie7));
            serviceHost.AddServiceEndpoint(typeof(IZadanie7), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad7");
            var behavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (behavior == null)
            {
                behavior = new ServiceMetadataBehavior();
            }
            serviceHost.Description.Behaviors.Add(behavior);
            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexNamedPipeBinding(), "net.pipe://localhost/metadata7");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
            Console.ReadLine();
        }
    }
}
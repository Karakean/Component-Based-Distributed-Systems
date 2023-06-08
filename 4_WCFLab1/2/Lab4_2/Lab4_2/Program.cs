using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
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
        return $"Zadanie 2 dziala. Podany argument: {arg}";
    }
}

namespace Lab4_2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie2));
            serviceHost.AddServiceEndpoint(typeof(IZadanie2), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad2");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
            Console.ReadLine();
        }
    }
}

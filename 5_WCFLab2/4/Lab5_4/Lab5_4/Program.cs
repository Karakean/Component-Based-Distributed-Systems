using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lab5_4
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Zadanie4 : KSR_WCF2.IZadanie4
    {
        private int counter;
        public int Dodaj(int param)
        {
            return counter = counter + param;
        }

        public void Ustaw(int arg)
        {
            counter = arg;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie4));
            serviceHost.AddServiceEndpoint(typeof(KSR_WCF2.IZadanie4), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf2-zad4");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}

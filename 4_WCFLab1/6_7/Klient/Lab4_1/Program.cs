using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSR_WCF1;
using System.ServiceModel;
using System.Threading;
using System.Runtime.Serialization;

namespace Lab4_5
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference.Zadanie7Client();
            try
            {
                client.RzucWyjatek7("Dzien dobry :)", 420);
            }
            catch (FaultException<ServiceReference.Wyjatek7> e)
            {
                Console.WriteLine(e.Message);
            }

            Thread.Sleep(3000);
            ((IDisposable)client).Dispose();
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lab5_3
{
    [ServiceContract]
    public interface IZadanie3
    {
        [OperationContract] void TestujZwrotny();
    }

    public class Zadanie3 : KSR_WCF2.IZadanie3
    {
        public void TestujZwrotny()
        {
            var callbackChannel = OperationContext.Current.GetCallbackChannel<KSR_WCF2.IZadanie3Zwrotny>();
            for (int i = 0; i <= 30; i++)
            {
                callbackChannel.WolanieZwrotne(i, i * i * i - i * i);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(Zadanie3));
            serviceHost.AddServiceEndpoint(typeof(KSR_WCF2.IZadanie3), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf2-zad3");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}

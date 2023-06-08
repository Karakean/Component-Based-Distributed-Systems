using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSR_WCF1;
using System.ServiceModel;
using System.Threading;

namespace Lab4_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IZadanie1> channelFactory = new ChannelFactory<IZadanie1>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/ksr-wcf1-test"));
            IZadanie1 client = channelFactory.CreateChannel();
            Console.WriteLine(client.Test("Zad 1 dziala! :)"));
            Thread.Sleep(3000);
            ((IDisposable)client).Dispose();
            channelFactory.Close();
            Console.ReadKey();
        }
    }
}

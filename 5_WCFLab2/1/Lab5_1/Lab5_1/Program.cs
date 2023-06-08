using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using KSR_WCF2;

namespace Lab5_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Zadanie1Client();
            IAsyncResult asyncResult = client.BeginDlugieObliczenia(null, null);
            for (int i = 0; i <= 20; i++)
                Console.WriteLine(client.Szybciej(i, 3 * i * i - 2 * i));
            Console.WriteLine(client.EndDlugieObliczenia(asyncResult));
            Console.ReadKey();
            ((IDisposable)client).Dispose();
        }
    }
}

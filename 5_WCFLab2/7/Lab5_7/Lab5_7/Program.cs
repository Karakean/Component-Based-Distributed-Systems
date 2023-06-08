using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lab5_7
{
    public class Zadanie6Callback : ServiceReference1.IZadanie6Callback
    {
        public void Wynik(int result)
        {
            Console.WriteLine($"Wynik: {result}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Zadanie5Client();
            Console.WriteLine(client.ScalNapisy(client.ScalNapisy("Zadanie ", "5,6,7 "), "Dziala :)"));
            Console.ReadKey();
            ((IDisposable)client).Dispose();
            var client1 = new ServiceReference1.Zadanie6Client(new InstanceContext(new Zadanie6Callback()));
            client1.Dodaj(2100, 37);
            Console.ReadKey();
            ((IDisposable)client1).Dispose();
        }
    }
}

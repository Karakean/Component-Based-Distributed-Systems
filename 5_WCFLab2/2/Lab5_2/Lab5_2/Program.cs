using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lab5_2
{
    public class Zad2Callback : ServiceReference1.IZadanie2Callback
    {
        public void Zadanie([MessageParameter(Name = "zadanie")] string zad, int x, bool zal)
        {
            Console.WriteLine($"{zad}. Punkty: {x}. Czy zaliczone: {zal}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Zadanie2Client(new InstanceContext(new Zad2Callback()));
            client.PodajZadania();
            Console.ReadKey();
            ((IDisposable)client).Dispose();
        }
    }
}

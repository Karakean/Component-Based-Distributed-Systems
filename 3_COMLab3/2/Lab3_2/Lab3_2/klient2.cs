using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab3_2
{
    class klient2
    {
        static void Main(string[] args)
        {
            Type type = Type.GetTypeFromProgID("KSR20.COM3Klasa.1");
            if (type != null)
            {
                try
                {
                    object x = Activator.CreateInstance(type);
                    type.InvokeMember("Test", System.Reflection.BindingFlags.InvokeMethod, null, x, new object[] { "ok!" });
                }
                catch
                {
                    Console.WriteLine("ERROR :(");
                }
            }
            else
            {
                Console.WriteLine("null type");
            }
            Thread.Sleep(2000);
        }
    }
}

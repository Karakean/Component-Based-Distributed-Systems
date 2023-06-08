using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab3_4
{
    class klient4
    {
        static void Main(string[] args)
        {
            klasacOut.IKlasa klasa = new klasacOut.Klasa();
            klasa.Test("klasaC OK!");
            Thread.Sleep(2000);
        }

    }
}

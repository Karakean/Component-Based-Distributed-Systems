using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Lab5_5
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie, usłudze i pliku konfiguracji.
    // UWAGA: aby uruchomić klienta testowego WCF w celu przetestowania tej usługi, wybierz plik Service1.svc lub Service1.svc.cs w eksploratorze rozwiązań i rozpocznij debugowanie.
    public class Service1 : KSR_WCF2.IZadanie5, KSR_WCF2.IZadanie6
    {
        public string ScalNapisy(string s1, string s2)
        {
            return s1 + s2;
        }

        public void Dodaj(int a, int b)
        {
            var callback = OperationContext.Current.GetCallbackChannel<KSR_WCF2.IZadanie6Zwrotny>();
            callback.Wynik(a + b);
        }
    }
}

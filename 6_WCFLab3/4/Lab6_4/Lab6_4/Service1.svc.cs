using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.IO;

namespace Lab6_4
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie, usłudze i pliku konfiguracji.
    // UWAGA: aby uruchomić klienta testowego WCF w celu przetestowania tej usługi, wybierz plik Service1.svc lub Service1.svc.cs w eksploratorze rozwiązań i rozpocznij debugowanie.
    public class Service1 : IService1
    {
        public XmlDocument GetIndex()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"C:\Users\mipig\Desktop\KSR\6_WCFLab3\index.xhtml");
            return xmlDocument;
        }

        public Stream GetScripts()
        {
            return new FileStream(@"C:\Users\mipig\Desktop\KSR\6_WCFLab3\scripts.js", FileMode.Open);
        }

        public int Dodaj(string parametr1, string parametr2)
        {
            return Int32.Parse(parametr1) + Int32.Parse(parametr2);
        }
    }
}

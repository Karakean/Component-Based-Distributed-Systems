using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.IO;

namespace Lab6_3
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę interfejsu „IService1” w kodzie i pliku konfiguracji.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract, WebGet(UriTemplate = "index.html"), XmlSerializerFormat]
        XmlDocument GetIndex();
        [OperationContract, WebGet(UriTemplate = "scripts.js")]
        Stream GetScripts();
    }
}

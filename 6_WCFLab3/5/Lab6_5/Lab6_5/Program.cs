using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.IO;
using System.Xml;

namespace Lab6_5
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract, WebGet(UriTemplate = "index.html"), XmlSerializerFormat]
        XmlDocument GetIndex();

        [OperationContract, WebGet(UriTemplate = "scripts.js")]
        Stream GetScripts();

        [OperationContract, WebInvoke(UriTemplate = "Dodaj/{parametr1}/{parametr2}")]
        int Dodaj(string parametr1, string parametr2);

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var channelFactory = new ChannelFactory<IService1>(new WebHttpBinding(), new EndpointAddress("http://localhost:64150/Service1.svc/Lab6_4"));
            channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            var client = channelFactory.CreateChannel();
            Console.WriteLine("Zadanie 5 dziala. 400 + 20 = " + client.Dodaj("400", "20"));
            ((IDisposable)client).Dispose();
            channelFactory.Close();
            Console.ReadKey();
        }
    }
}

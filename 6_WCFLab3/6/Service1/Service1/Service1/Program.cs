﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Service1
{
    [ServiceContract]
    public interface IZadanie6
    {
        [OperationContract]
        int Dodaj(int a, int b);
    }
    public class Zadanie6 : IZadanie6
    {
        public int Dodaj(int a, int b)
        {
            Console.WriteLine("Service1");
            return a + b;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Zadanie6));
            serviceHost.AddServiceEndpoint(typeof(IZadanie6), new NetNamedPipeBinding(), "net.pipe://localhost/service1");
            serviceHost.Open();
            Console.WriteLine("Service1 dziala na: net.pipe://localhost/service1");
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}

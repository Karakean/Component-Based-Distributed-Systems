using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static string Result(bool result, bool expected)
        {
            return result == expected ? "Success" : "Fail";
        }
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            Console.WriteLine("Creation1: " + Result(client.Create("user1", "pa$$word1"), true));
            Console.WriteLine("Creation2: " + Result(client.Create("user2", "pa$$word2"), true));
            Console.WriteLine("IncorrectCreation: " + Result(client.Create("user1", "pa$$word2"), false));
            Guid guid = client.Login("user1", "pa$$word1");
            Console.WriteLine("Login1: " + guid);
            Guid guid1 = client.Login("user2", "pa$$word2");
            Console.WriteLine("Login2: " + guid1);
            Guid incorrect = client.Login("inc", "inc");
            Console.WriteLine("IncorrectLogin: " + incorrect);
            Console.WriteLine("PutResult1: " + Result(client.Put("plik.txt", "contentcontent", guid), true));
            Console.WriteLine("PutResult2: " + Result(client.Put("plik.txt", "differentdifferent", guid1), true));
            Console.WriteLine("Content1: " + client.Get("plik.txt", guid));
            Console.WriteLine("Content2: " + client.Get("plik.txt", guid1));
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static string Assert(bool result, bool expected)
        {
            return result == expected ? "Success" : "Fail";
        }
        static void Main(string[] args)
        {
            ServiceReference2.Service1Client client = new ServiceReference2.Service1Client();
            //client.InitializeDatabase();
            string user1Login = "TEST1";
            string user1Password = "pa$$word1";
            string user2Login = "TEST2";
            string user2Password = "pa$$word2";
            Console.WriteLine("Creation1: " + Assert(client.Create(user1Login, user1Password), true));
            Console.WriteLine("Creation2: " + Assert(client.Create(user2Login, user2Password), true));
            Console.WriteLine("IncorrectCreation: " + Assert(client.Create(user1Login, user2Password), false));
            Guid guid1 = client.Login(user1Login, user1Password);
            Console.WriteLine("User1 Login: " + guid1);
            Console.WriteLine("User1 Logout: " + Assert(client.Logout(user1Login), true));
            guid1 = client.Login(user1Login, user1Password);
            Console.WriteLine("Non-existing user logout: " + Assert(client.Logout("non-existing"), false));
            Guid guid2 = client.Login(user2Login, user2Password);
            Console.WriteLine("User2 Login: " + guid2);
            Guid incorrect = client.Login("inc", "inc");
            Console.WriteLine("IncorrectLogin: " + incorrect);
            Console.WriteLine("PutAssert1: " + Assert(client.Put("plik.txt", "contentcontent", guid1), true));
            Console.WriteLine("PutAssert2: " + Assert(client.Put("plik.txt", "differentdifferent", guid2), true));
            Console.WriteLine("Content1: " + client.Get("plik.txt", guid1));
            Console.WriteLine("Content2: " + client.Get("plik.txt", guid2));
            Console.ReadKey();
        }
    }
}

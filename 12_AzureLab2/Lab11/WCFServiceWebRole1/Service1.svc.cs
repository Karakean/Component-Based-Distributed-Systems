using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace WCFServiceWebRole1
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie, usłudze i pliku konfiguracji.
    // UWAGA: aby uruchomić klienta testowego WCF w celu przetestowania tej usługi, wybierz plik Service1.svc lub Service1.svc.cs w eksploratorze rozwiązań i rozpocznij debugowanie.
    public class Service1 : IService1
    {
        public bool InitializeDatabase()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "create table users (id int identity(1,1) primary key, login varchar(100), password varchar(100), session_id varchar(100)); " +
                                      "create table files (id int identity(1,1) primary key, name varchar(100), content varchar(200)); ";
                command.ExecuteNonQuery();
                return true;
            }
        }
        public bool Create(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select count(*) from users where login = '{login}';";
                int count = (Int32)command.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
                command.CommandText = $"insert into users values('{login}', '{password}', '{Guid.Empty}')";
                command.ExecuteNonQuery();
                return true;
            }
        }

        public Guid Login(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select count(*) from users where login = '{login}' and password = '{password}';";
                int count = (Int32)command.ExecuteScalar();
                if (count == 0)
                {
                    return Guid.Empty;
                }
                Guid session_id = Guid.NewGuid();
                command.CommandText = "UPDATE users SET session_id = @SessionId WHERE login = @Login AND password = @Password;";
                command.Parameters.AddWithValue("@SessionId", session_id);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                command.ExecuteNonQuery();
                return session_id;
            } 
        }

        public bool Logout(string login)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select count(*) from users where login = '{login}';";
                int count = (Int32)command.ExecuteScalar();
                if (count == 0)
                {
                    return false;
                }
                command.CommandText = $"update users set session_id = {Guid.Empty} where login = '{login}';";
                command.ExecuteNonQuery();
                return true;
            }
        }

        public bool Put(string name, string content, Guid sessionId)
        {
            if(sessionId == Guid.Empty)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select login from users where session_id = '{sessionId}';";
                string login = (string)command.ExecuteScalar();
                if (login == null || login == String.Empty)
                {
                    return false;
                }
                string uniqueName = login + name;
                command.CommandText = $"select count(*) from files where name = '{uniqueName}';";
                int count = (Int32)command.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
                command.CommandText = $"insert into files values('{uniqueName}', '{content}')";
                command.ExecuteNonQuery();
                return true;
            }
        }

        public string Get(string name, Guid sessionId)
        {
            if (sessionId == Guid.Empty)
            {
                return String.Empty;
            }
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=AzureLab2;Trusted_Connection=yes;";
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select login from users where session_id = '{sessionId}';";
                string login = (string)command.ExecuteScalar();
                if (login == null || login == String.Empty)
                {
                    return String.Empty;
                }
                string uniqueName = login + name;
                command.CommandText = $"select count(*) from files where name = '{uniqueName}';";
                int count = (Int32)command.ExecuteScalar();
                if (count == 0)
                {
                    return String.Empty;
                }
                command.CommandText = $"select content from files where name = '{uniqueName}';";
                return command.ExecuteScalar().ToString();
            }
        }
    }
}

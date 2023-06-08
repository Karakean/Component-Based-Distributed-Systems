using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WCFServiceWebRole1
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie, usłudze i pliku konfiguracji.
    // UWAGA: aby uruchomić klienta testowego WCF w celu przetestowania tej usługi, wybierz plik Service1.svc lub Service1.svc.cs w eksploratorze rozwiązań i rozpocznij debugowanie.
    public class Service1 : IService1
    {
        public bool Create(string login, string password)
        {
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable usersTable = tableClient.GetTableReference("users");
            usersTable.CreateIfNotExists();

            var retrieveOperation = TableOperation.Retrieve<User>("partKey" + login, "rowKey" + login);
            var retrieveResult = usersTable.Execute(retrieveOperation);

            if (retrieveResult.Result != null)
            {
                return false;
            }

            var user = new User("rowKey" + login, "partKey" + login)
            {
                Login = login,
                Password = password,
                SessionId = Guid.Empty
            };

            var insertOperation = TableOperation.Insert(user);
            var insertResult = usersTable.Execute(insertOperation);

            return insertResult.Result != null;
        }

        public Guid Login(string login, string password)
        {
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable usersTable = tableClient.GetTableReference("users");
            usersTable.CreateIfNotExists();

            var retrieveOperation = TableOperation.Retrieve<User>("partKey" + login, "rowKey" + login);
            var retrieveResult = usersTable.Execute(retrieveOperation);
            var user = retrieveResult.Result as User;

            if (user == null)
            {
                return Guid.Empty;
            }

            var sessionId = Guid.NewGuid();
            user.SessionId = sessionId;

            var replaceOperation = TableOperation.Replace(user);
            usersTable.Execute(replaceOperation);

            return sessionId;
        }

        public bool Logout(string login)
        {
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable usersTable = tableClient.GetTableReference("users");
            usersTable.CreateIfNotExists();

            TableQuery<User> query = new TableQuery<User>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "partKey" + login));
            var queryResult = usersTable.ExecuteQuery(query);
            var user = queryResult.SingleOrDefault();

            if (user == null)
            {
                return false;
            }

            user.SessionId = Guid.Empty;
            var replaceOperation = TableOperation.Replace(user);
            usersTable.Execute(replaceOperation);

            return true;
        }

        public bool Put(string name, string value, Guid sessionId)
        {
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable usersTable = tableClient.GetTableReference("users");
            usersTable.CreateIfNotExists();

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("files");
            container.CreateIfNotExists();

            TableQuery<User> query = new TableQuery<User>()
                .Where(TableQuery.GenerateFilterConditionForGuid("SessionId", QueryComparisons.Equal, sessionId));
            var queryResult = usersTable.ExecuteQuery(query);
            var user = queryResult.SingleOrDefault();

            if (user == null)
            {
                return false;
            }

            var blob = container.GetBlockBlobReference(user.Login + name);

            var bytes = Encoding.ASCII.GetBytes(value);
            var stream = new MemoryStream(bytes);
            blob.UploadFromStream(stream);

            return true;
        }

        public string Get(string name, Guid sessionId)
        {
            CloudStorageAccount account = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable usersTable = tableClient.GetTableReference("users");
            usersTable.CreateIfNotExists();

            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("files");
            container.CreateIfNotExists();

            TableQuery<User> query = new TableQuery<User>()
                .Where(TableQuery.GenerateFilterConditionForGuid("SessionId", QueryComparisons.Equal, sessionId));
            var queryResult = usersTable.ExecuteQuery(query);
            var user = queryResult.SingleOrDefault();

            if (user == null)
            {
                return string.Empty;
            }

            var blob = container.GetBlockBlobReference(user.Login + name);
            if (blob == null)
            {
                return string.Empty;
            }

            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                string content = Encoding.UTF8.GetString(stream.ToArray());
                return content;
            }
        }
    }
}

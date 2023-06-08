using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Library.Web.Utilities
{
    internal static class RequestHandler
    {
        public static async Task<T> MakeRequest<T>(string endpointAddress) where T : class
        {
            var client = new HttpClient();
            var serializer = new DataContractJsonSerializer(typeof(T));
        
            Console.WriteLine($"Calling endpoint: {endpointAddress}");
            var response = await client.GetStreamAsync(endpointAddress);
            var result = serializer.ReadObject(response) as T;
            
            return result;
        }
    }
}
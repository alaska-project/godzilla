using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.FunctionalTests
{
    public static class HttpClientExtensions
    {
        public static async Task PostJsonAsync(this HttpClient client, string uri, object body)
        {
            var response = await client.PostAsync(uri, JsonContent(body));

            response.EnsureSuccessStatusCode();
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string uri, object body)
        {
            var response = await client.PostAsync(uri, JsonContent(body));

            return await response.ReadAsAsync<T>();
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string uri)
        {
            var response = await client.PostAsync(uri, null);

            return await response.ReadAsAsync<T>();
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string uri)
        {
            var response = await client.GetAsync(uri);

            return await response.ReadAsAsync<T>();
        }

        public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage responseMessage)
        {
            responseMessage.EnsureSuccessStatusCode();

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        private static HttpContent JsonContent(object item)
        {
            var serializedItem = JsonConvert.SerializeObject(item);
            return new StringContent(serializedItem, Encoding.UTF8, "application/json");
        }
    }
}

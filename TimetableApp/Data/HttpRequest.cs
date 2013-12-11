using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TimetableApp.Data
{
    public class HttpRequest
    {
        public async static Task<HttpResponseMessage> Post(string url, object data, string accessToken = null, string mediaValueType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var jsonData = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            if (accessToken != null)
            {
                client.DefaultRequestHeaders.Add("X-accessToken", accessToken);
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(request);

            return response;
        }

        public async static Task<T> Post<T>(string url, object data, string accessToken = null, string mediaValueType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var jsonData = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (accessToken != null)
            {
                client.DefaultRequestHeaders.Add("X-accessToken", accessToken);
            }

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<T>(content);

            return responseData;
        }

        public async static Task<T> Get<T>(string url, string accessToken, string mediaValueType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-accessToken", accessToken);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<T>(content);
            return responseData;
        }

        public async static Task<HttpResponseMessage> Delete(string url, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-accessToken", accessToken);
            var response = await client.SendAsync(request);

            return response;
        }

        public async static Task<HttpResponseMessage> Put(string url, string accessToken, string mediaValueType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-accessToken", accessToken);
            var response = await client.SendAsync(request);

            return response;
        }
    }
}
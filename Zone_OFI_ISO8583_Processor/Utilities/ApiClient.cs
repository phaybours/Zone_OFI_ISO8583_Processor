using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace Zone_OFI_ISO8583_Processor.Utilities
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> PostAsync<T>(string url, T data)
        {
            var connectionDetails = ZNConnection.GetDetiails();
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(data)
            };
            request.Headers.Add("accept", "application/json");
            request.Headers.Add("x-api-key", connectionDetails.APIKey);

            var response = await _httpClient.SendAsync(request);
            //var res = await response.Content.ReadAsStringAsync();//.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

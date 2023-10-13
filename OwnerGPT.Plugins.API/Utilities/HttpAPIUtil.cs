using System.Net.Http.Json;
using System.Text.Json;

namespace OwnerGPT.Plugins.Parsers.API.Utilities
{
    public class HttpAPIUtil
    {
        private readonly IHttpClientFactory HttpClientFactory;

        public HttpAPIUtil(IHttpClientFactory httpClientFactory) =>
            HttpClientFactory = httpClientFactory;

        public HttpClient GetClientInstance() =>
            HttpClientFactory.CreateClient();
        
        public async Task<T> GetAsync<T>(string endpoint) =>
            Deserialize<T>(await (await GetClientInstance().GetAsync(endpoint)).Content.ReadAsStringAsync());
        

        public async Task<T> PostAsync<T, B>(string endpoint, B body) =>
            Deserialize<T>(await (await GetClientInstance().PostAsJsonAsync(endpoint, body)).Content.ReadAsStringAsync());

        private T Deserialize<T>(string json) =>
            JsonSerializer.Deserialize<T>(json)!;
    }
}

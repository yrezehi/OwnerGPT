using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Parsers.API.Utilities
{
    public class HttpAPIUtil
    {
        private readonly IHttpClientFactory HttpClientFactory;

        public HttpAPIUtil(IHttpClientFactory httpClientFactory) {
            HttpClientFactory = httpClientFactory;
        }

        public HttpClient GetClientInstance() =>
            HttpClientFactory.CreateClient();
        
        public async Task<T> GetAsync<T>(string endpoint)
        {
            return Deserialize<T>(await (await GetClientInstance().GetAsync(endpoint)).Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T, B>(string endpoint, B body)
        {
            return Deserialize<T>(await (await GetClientInstance().PostAsJsonAsync(endpoint, body)).Content.ReadAsStringAsync());
        }

        private T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }

    }
}

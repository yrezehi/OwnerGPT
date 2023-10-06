using System.Collections.Concurrent;

namespace OwnerGPT.Plugins.Parsers.Web.Crawlers
{
    public class CrawlersCoordinator
    {
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly ConcurrentDictionary<string, List<string>> ProgressiveJobs;

        public CrawlersCoordinator(IHttpClientFactory httpClientFactory) =>
            (HttpClientFactory, ProgressiveJobs) = (httpClientFactory, new());

        public HttpClient GetClient() => 
            HttpClientFactory.CreateClient();
    }
}

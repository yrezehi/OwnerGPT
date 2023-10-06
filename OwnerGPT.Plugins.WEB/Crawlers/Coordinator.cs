namespace OwnerGPT.Plugins.Parsers.Web.Crawlers
{
    public class CrawlersCoordinator
    {
        private IHttpClientFactory HttpClientFactory;

        public CrawlersCoordinator(IHttpClientFactory httpClientFactory) =>
            HttpClientFactory = httpClientFactory;
    }
}

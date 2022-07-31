namespace StrategyManager.Infrastructure.Proxies.Clients
{
    public class BinanceHttpClient
    {
        public HttpClient Client { get; private set; }

        public BinanceHttpClient(HttpClient httpClient)
        {
            this.Client = httpClient;
        }
    }
}

using StrategyManager.Core.Models.Proxies.Clients;
using StrategyManager.Core.Proxies.Clients;
using StrategyManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace StrategyManager.Infrastructure.Proxies.Clients
{
    public class BinanceApiClient : IBinanceApiClient
    {
        public BinanceHttpClient httpClient { get; private set; }
        private readonly BinanceApiClientOptions options;
        private const string AccessTokenRoute = "token";

        public BinanceApiClient(BinanceHttpClient httpClient, IOptions<BinanceApiClientOptions> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        public async Task<BinanceResponse> RequestAccessTokenAsync(string authorizationCode)
        {
            if (string.IsNullOrEmpty(options.Url)) throw new ArgumentNullException(nameof(options.Url));
            
            var dictionary = new Dictionary<string, string>();
            //dictionary.Add("code", authorizationCode);
            //dictionary.Add("client_secret", options.ClientSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, AccessTokenRoute) { Content = new FormUrlEncodedContent(dictionary) };
            var response = await httpClient.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var accessToken = JsonSerializer.Deserialize<BinanceResponse>(content);

            if (accessToken is null) throw new Exception("Access token is null");

            return accessToken;
        }
    }
}

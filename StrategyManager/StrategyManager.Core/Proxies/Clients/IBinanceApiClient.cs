using StrategyManager.Core.Models.Proxies.Clients;

namespace StrategyManager.Core.Proxies.Clients
{
    public interface IBinanceApiClient
    {
        Task<BinanceResponse> RequestAccessTokenAsync( string authorizationCode);
    }
}

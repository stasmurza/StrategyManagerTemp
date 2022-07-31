using System.Security.Claims;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IJwtTokenService
    {
        string GetToken(IReadOnlyCollection<Claim> claims);
    }
}

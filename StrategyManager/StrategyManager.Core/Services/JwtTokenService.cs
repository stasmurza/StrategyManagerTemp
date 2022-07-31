using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StrategyManager.Core.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenOptions options;

        public JwtTokenService(IOptions<JwtTokenOptions> options)
        {
            this.options = options.Value;
        }

        public string GetToken(IReadOnlyCollection<Claim> claims)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            if (claims.Any())
            {
                authClaims.AddRange(claims);
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(options.ExpirationTimeMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

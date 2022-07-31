using StrategyManager.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Authentication injection
    /// </summary>
    public static class AuthenticationInjection
    {
        /// <summary>
        /// Add authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var secret = configuration.GetValue<string>(EnvVariableNameConstants.JwtSecret);
                    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = symmetricKey
                    };
                });

            return services;
        }
    }
}

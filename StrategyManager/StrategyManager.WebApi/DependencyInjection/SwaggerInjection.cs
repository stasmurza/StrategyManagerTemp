using StrategyManager.Contracts;
using StrategyManager.WebAPI.Configuration.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Swagger injection
    /// </summary>
    public static class SwaggerInjection
    {
        /// <summary>
        /// Add swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new()
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new()
                {
                    {
                        new()
                        {
                            Reference = new()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                ApiVersions.Versions.ForEach(version => c.SwaggerDoc(version.Version, version));
                c.AddXmlComments(new[] { Assembly.GetExecutingAssembly(), typeof(ContractBase).Assembly });
                c.DescribeAllParametersInCamelCase();
            });

            return services;
        }

        /// <summary>
        /// Add xml comments
        /// </summary>
        /// <param name="options"></param>
        /// <param name="assemblies"></param>
        public static void AddXmlComments(this SwaggerGenOptions options, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            }
        }
    }
}

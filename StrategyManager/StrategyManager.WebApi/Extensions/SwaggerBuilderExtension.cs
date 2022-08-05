using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace StrategyManager.WebAPI.Extensions
{
    /// <summary>
    /// Swagger builder extensions
    /// </summary>
    public static class SwaggerBuilderExtensions
    {
        /// <summary>
        /// Setup swagger UI
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="apiVersions"></param>
        public static void SetupSwaggerUI(this IApplicationBuilder applicationBuilder, List<OpenApiInfo> apiVersions)
        {
            applicationBuilder.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                if (!httpRequest.Headers.ContainsKey("X-Forwarded-Prefix")) return;

                var serverUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{httpRequest.Headers["X-Forwarded-Prefix"]}";

                swaggerDoc.Servers = new List<OpenApiServer>() { new OpenApiServer { Url = serverUrl } };
            }));
            applicationBuilder.UseSwaggerUI(options => SetupSwaggerUIOptions(options, apiVersions));
        }

        private static void SetupSwaggerUIOptions(SwaggerUIOptions options, List<OpenApiInfo> apiVersions)
        {
            apiVersions.ForEach(version =>
            {
                options.SwaggerEndpoint($"swagger/{version.Version}/swagger.json", version.Title);
                options.RoutePrefix = "";
            });
        }
    }
}

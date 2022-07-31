using Microsoft.AspNetCore.Mvc;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Api version injection
    /// </summary>
    public static class ApiVersionInjection
    {
        /// <summary>
        /// Add versioning
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void AddVersioning(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            serviceCollection.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}

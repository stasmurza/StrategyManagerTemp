using Microsoft.OpenApi.Models;

namespace StrategyManager.WebAPI.Configuration.Swagger
{
    public class ApiVersions
    {
        public const string V1 = "1";

        public static List<OpenApiInfo> Versions => new()
        {
            new OpenApiInfo
            {
                Version = "v1",
                Title = "Strategy manager API V1",
            }
        };
    }
}

using StrategyManager.Core.Models.Options;
using StrategyManager.Infrastructure;
using StrategyManager.Infrastructure.Options;

namespace StrategyManager.WebAPI.Configuration
{
    /// <summary>
    /// Configure options
    /// </summary>
    public static class OptionsConfiguration
    {
        /// <summary>
        /// Add business layer options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessLayerServiceOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtTokenOptions>(options =>
            {
                options.Secret = config.GetValue<string>(EnvVariableNameConstants.JwtSecret);
                options.ExpirationTimeMinutes = config.GetValue<int>(EnvVariableNameConstants.JwtExpirationTimeMinutes);
            });

            services.Configure<EmailOptions>(options =>
            {
                options.EmailFromAddress = config.GetValue<string>(EnvVariableNameConstants.EmailFromAddress);
            });

            return services;
        }

        /// <summary>
        /// Add infrastructure options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServiceOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DatabaseOptions>(options =>
            {
                options.ConnectionString = config.GetValue<string>(EnvVariableNameConstants.StrategyManagerDbConnection);
                options.DatabaseName = config.GetValue<string>(EnvVariableNameConstants.StrategyManagerDbName);
            });

            services.Configure<RabbitMQClientOptions>(options =>
            {
                options.HostName = config.GetValue<string>(EnvVariableNameConstants.RabbitMQHostName);
                options.UserName = config.GetValue<string>(EnvVariableNameConstants.RabbitMQUserName);
                options.Password = config.GetValue<string>(EnvVariableNameConstants.RabbitMQPassword);
                options.QueueName = config.GetValue<string>(EnvVariableNameConstants.RabbitMQQueueName);
            });

            return services;
        }

    }
}

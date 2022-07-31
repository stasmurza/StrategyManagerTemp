using StrategyManager.WebAPI.Mapping;
using AutoMapper;
using StrategyManager.Core.Mapping;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Auto mapper service injection
    /// </summary>
    public static class AutoMapperServiceInjection
    {
        /// <summary>
        /// Inject Automapper
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapping(this IServiceCollection services)
        {
            services.AddSingleton<Profile, ContractsMapping>();
            services.AddSingleton<Profile, EntityMapping>();

            services.AddSingleton(sp => new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(sp.GetServices<Profile>());
            }).CreateMapper());

            return services;
        }
    }
}

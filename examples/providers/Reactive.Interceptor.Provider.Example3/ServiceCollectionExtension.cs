using Microsoft.Extensions.DependencyInjection;

namespace Reactive.Interceptor.Provider.Example3
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReactiveProviderExample3(this IServiceCollection services)
        {
            services.AddTransient<ProviderExample3>();

            return services;
        }
    }
}

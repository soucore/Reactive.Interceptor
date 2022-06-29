using Microsoft.Extensions.DependencyInjection;

namespace Reactive.Interceptor.Provider.Example2
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReactiveProviderExample2(this IServiceCollection services)
        {
            services.AddTransient<ProviderExample2>();

            return services;
        }
    }
}

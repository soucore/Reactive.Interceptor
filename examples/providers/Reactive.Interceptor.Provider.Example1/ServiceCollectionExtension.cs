using Microsoft.Extensions.DependencyInjection;

namespace Reactive.Interceptor.Provider.Example1
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReactiveProviderExample1(this IServiceCollection services)
        {
            services.AddTransient<ProviderExample1>();

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reactive.Interceptor.Provider.Example1
{
    public static class ServiceCollectionExtension
    {
        public static IHostBuilder UseReactiveProviderExample1(this IHostBuilder builder)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddTransient<ProviderExample1>();
            });

            return builder;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reactive.Interceptor.Provider.Example2
{
    public static class ServiceCollectionExtension
    {
        public static IHostBuilder UseReactiveProviderExample2(this IHostBuilder builder)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddTransient<ProviderExample2>();
            });

            return builder;
        }
    }
}

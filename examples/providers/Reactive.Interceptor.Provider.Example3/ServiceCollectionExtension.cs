using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reactive.Interceptor.Provider.Example3
{
    public static class ServiceCollectionExtension
    {
        public static IHostBuilder UseReactiveProviderExample3(this IHostBuilder builder)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddTransient<ProviderExample3>();
            });

            return builder;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reactive.Interceptor.Extensions;
public static class HostBuilderExtension
{

    public static IHostBuilder UseReactiveInterceptor(this IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureServices((hostBuilder, services) =>
        {
            var provider = services.BuildServiceProvider();
            var b = provider.GetRequiredService<Bootstrap>();
            b.Build();
        });

        return builder;
    }
}

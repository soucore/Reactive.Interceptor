using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reactive.Interceptor.Core.Configurations;

namespace Reactive.Interceptor;
public static class HostBuilderExtension
{

    public static IHostBuilder UseReactiveInterceptor<T>(this IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureServices((HostBuilderContext hostBuilder, IServiceCollection services) =>
        {
            services.Configure<ConfigurationClient>(hostBuilder.Configuration.GetSection(ConfigurationClient.Key));
            ConfigureServices(services, typeof(T));
        });

        return builder;
    }

    public static IHostBuilder UseReactiveInterceptor(this IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ConfigureServices((HostBuilderContext hostBuilder, IServiceCollection services) =>
        {
            services.Configure<ConfigurationClient>(hostBuilder.Configuration.GetSection(ConfigurationClient.Key));
            ConfigureServices(services);
        });
        return builder;
    }

    private static void ConfigureServices(IServiceCollection services, Type type = null)
    {
        services.Inject(type);
        var providers = services.BuildServiceProvider();
        var b = ActivatorUtilities.CreateInstance<Bootstrap>(providers);
        b.Build(type);

    }
}

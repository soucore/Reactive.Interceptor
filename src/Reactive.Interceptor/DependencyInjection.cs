using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Reactive.Interceptor.Client;
using Reactive.Interceptor.Client.Interface;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Core.Configurations;
using Reactive.Interceptor.Core.Interfaces;
using Reactive.Interceptor.Providers;

namespace Reactive.Interceptor;
public static class DependencyInjection
{
    public static IServiceCollection Inject<T>(this IServiceCollection services, IConfiguration configuration)
    {
        Inject(services, configuration);
        Type type = typeof(T);
        if (type is not null)
            services.TryAddSingleton(type);

        return services;
    }

    public static IServiceCollection Inject(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConfigurationClient>(configuration.GetSection(ConfigurationClient.Key));
        services.TryAddSingleton(typeof(IConfigurationProvider<>), typeof(ConfigurationProvider<>));
        services.TryAddSingleton<Bootstrap>();
        services.TryAddSingleton<Context>();
        services.TryAddSingleton<IPolicyStrategy, PolicyStrategy>();
        services.TryAddSingleton<IEventMediator, EventMediator>();
        services.TryAddSingleton<IProviderReflection, ProviderReflection>();
        services.TryAddSingleton<IInterceptorReflection, InterceptorReflection>();

        return services;
    }
}

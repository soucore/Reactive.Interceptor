using Microsoft.Extensions.DependencyInjection;
using Reactive.Interceptor.Client;
using Reactive.Interceptor.Client.Interface;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Core.Interfaces;
using Reactive.Interceptor.Providers;

namespace Reactive.Interceptor;
public static class InjectInternalDependencies
{
    public static IServiceCollection Inject(this IServiceCollection services, Type type = null)
    {
        services.AddSingleton(typeof(IConfigurationProvider<>), typeof(ConfigurationProvider<>));
        services.AddScoped<Context>();
        services.AddTransient<IPolicyStrategy, PolicyStrategy>();
        services.AddTransient<IEventMediator, EventMediator>();
        services.AddTransient<IProviderReflection, ProviderReflection>();
        services.AddTransient<IInterceptorReflection, InterceptorReflection>();

        if(type is not null)
            services.AddTransient(type);

        return services;
    }
}

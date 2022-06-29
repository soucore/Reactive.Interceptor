using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reactive.Interceptor.Client.Interface;
using Reactive.Interceptor.Core.Interfaces;

namespace Reactive.Interceptor;
public class Bootstrap
{
    private readonly ILogger _log;
    private readonly IInterceptorReflection _interceptor;
    private readonly IHostApplicationLifetime _hostApp;
    private IProviderReflection _provider { get; set; }
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }

    private static Type Type;

    public Bootstrap(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }

    public Bootstrap(IHostApplicationLifetime hostApp,
        ILoggerFactory loggerFactory, 
        IProviderReflection provider,
        IInterceptorReflection interceptor)
    {
        _log = loggerFactory.CreateLogger("Reactive.Interceptor");
        _hostApp = hostApp;
        _provider = provider;
        _interceptor = interceptor;
    }

    public Bootstrap Inject<T>()
    {
        Type = typeof(T);
        DependencyInjection.Inject<T>(Services, Configuration);
        return this;
    }

    public Bootstrap Inject()
    {
        DependencyInjection.Inject(Services, Configuration);
        return this;
    }

    public void Build()
    {
        _log.LogDebug("Started construction of the Interceptor!");
        _interceptor.Run(Type);
        _provider.Run(Type, _hostApp.ApplicationStopping);
    }
}

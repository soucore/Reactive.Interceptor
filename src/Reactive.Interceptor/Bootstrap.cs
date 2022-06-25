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

    public void Build(Type type)
    {
        _log.LogDebug("Started construction of the Interceptor!");

        _interceptor.Run(type);
        _provider.Run(type, _hostApp.ApplicationStopping);
    }
}

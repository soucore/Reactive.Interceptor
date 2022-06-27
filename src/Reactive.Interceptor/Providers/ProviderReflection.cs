using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Core.Configurations;
using Reactive.Interceptor.Core.Extensions;
using Reactive.Interceptor.Core.Interfaces;
using Reactive.Interceptor.Providers.Interface;
using System.Reflection;

namespace Reactive.Interceptor.Providers;

public class ProviderReflection : IProviderReflection
{
    private CancellationToken CancellationToken;

    private readonly ILogger<ProviderReflection> _log;
    private readonly ConfigurationClient _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventMediator _event;
    private readonly IPolicyStrategy _policyCore;
    private Context _context;
    private IProviderBase ProviderSource;
    private IProviderBase ProviderSink; 
    private IProviderBase ProviderDeadLetter;
    private Type InterceptorType;


    public ProviderReflection(ILogger<ProviderReflection> logger, 
        Context context,
        IOptions<ConfigurationClient> configuration,
        IServiceProvider serviceProvider, 
        IEventMediator eventWrapper,
        IPolicyStrategy policyCore)
    {
        _log = logger;
        _context = context;
        _configuration = configuration.Value;
        _serviceProvider = serviceProvider;
        _event = eventWrapper;
        _policyCore = policyCore;
    }

    public async Task Run(Type interceptorType, CancellationToken cancellationToken)
    {
        InterceptorType = interceptorType;
        CancellationToken = cancellationToken;
        try
        {
            var types = DiscoverProviders();
            await BuildProviders(types);
            ListenProvider();
        }
        catch (Exception ex)
        {
            var err = new CoreException(ex.Message, ex);
            _log.LogError(err.ToString());
            _context.Exceptions.Add(err);

            await _event.EmitFromProviderToInterceptorError(err);
        }
    }
    public IEnumerable<Type> DiscoverProviders()
    {
        IEnumerable<AssemblyName> assemblies = Assembly.GetEntryAssembly()
            .GetReferencedAssemblies()
            .Where(x => x.FullName.Contains("Reactive.Interceptor.Provider"));

        List<Type> types = new();
        foreach (var assemblyName in assemblies)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            IEnumerable<Type> type = assembly.GetTypes()
                .Where(type => type.GetInterface(typeof(IProviderBase).Name, true) is not null);

            types.AddRange(type);
        }

        return types;
    }

    public async Task BuildProviders(IEnumerable<Type> providerTypes)
    {
        foreach (var type in providerTypes)
            await BuildType(type);

        if (ProviderSource is null) 
            throw new InvalidOperationException("Provider source not found!");

        if (ProviderSink is null) 
            throw new InvalidOperationException("Provider sink not found!");

        if (ProviderDeadLetter is null)
            throw new InvalidOperationException("Provider dead letter not found!"); 
    }

    public async Task BuildType(Type type)
    {
        _log.LogDebug($"Provider found: {type.Name}");
        IProviderBase provider = (IProviderBase)ActivatorUtilities.CreateInstance(_serviceProvider, type);
        var providerName = type.GetProperty("ProviderName").GetValue(provider).ToString();

        _ = ExcecuteIfExistSource(provider, providerName);

        if (_configuration.Sink.Provider.Equals(providerName.ToString(), StringComparison.OrdinalIgnoreCase))
            ProviderSink = provider;

        if (_configuration.DeadLetter.Provider.Equals(providerName.ToString(), StringComparison.OrdinalIgnoreCase))
            ProviderDeadLetter = provider;

        await Task.CompletedTask;
    }

    public async Task ExcecuteIfExistSource(IProviderBase provider, string providerName)
    {
        if (_configuration.Source.Provider.Equals(providerName, StringComparison.OrdinalIgnoreCase))
        {
            ProviderSource = provider;
            _ = Task.Factory.StartNew(
                () => ProviderSource.InvokeMethodAsync("StartAsync", CancellationToken)
                , TaskCreationOptions.LongRunning
            );
        }

        await Task.CompletedTask;
    }

    private void ListenProvider()
    {
        ProviderSource.EventSource += SendDataConsumed;
        if (InterceptorType is not null)
            EventMediator.OnInterceptorToProviderSink += InvokeSink;
    }

    private void SendDataConsumed(object data)
    {
        _context = Context.Clear();
        _context.DataInputOriginal = data;
        if(InterceptorType is null)
            _ = InvokeSink(data);
        else
            _event.EmitFromProviderToInterceptorAfterConsumed(data);
    }

    private async Task InvokeSink(object data)
    {
        try
        {
            var strategy = _policyCore.BuildStrategy();
            var result = await strategy.ExecuteAsync(async () =>
            {
                var response = (Response) await ProviderSink.InvokeTaskMethodResultAsync("Sink", data);
                if(!response.Published && response.ExceptionCore is not null)
                    throw new CoreException("Fail Retry!", response.ExceptionCore);

                return response;
            });

            _context.Response = result;
            if (InterceptorType is not null)
                _ = _event.EmitFromProviderToInterceptorAfterPublished(_context);
        }
        catch (Exception ex)
        {
            var err = new CoreException(ex.Message, ex);
            _log.LogCritical(err.ToString());

            _event.EmitBreakToProvider(false);

            if (InterceptorType is not null)
                _ = _event.EmitFromProviderToInterceptorError(err);

            _ = ProviderDeadLetter.InvokeMethodAsync("Sink", data);
        }

        await Task.CompletedTask;
    }
}

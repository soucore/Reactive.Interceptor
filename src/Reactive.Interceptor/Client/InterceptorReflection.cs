using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reactive.Interceptor.Client.Interface;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Core.Extensions;
using Reactive.Interceptor.Core.Helpers;
using Reactive.Interceptor.Core.Interfaces;

namespace Reactive.Interceptor.Client;
public class InterceptorReflection : IInterceptorReflection
{
    private readonly ILogger<InterceptorReflection> _log;
    private readonly Context _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventMediator _eventWrapper;
    private object Interceptor;

    public InterceptorReflection(ILogger<InterceptorReflection> logger,
        Context context,
        IServiceProvider serviceProvider,
        IEventMediator eventWrapper)
    {
        _log = logger;
        _context = context;
        _serviceProvider = serviceProvider;
        _eventWrapper = eventWrapper;
    }

    public async Task Run(Type type)
    {
        if (type is null) return;
        Interceptor = _serviceProvider.GetRequiredService(type);
        ListenEvents();

        await Task.CompletedTask;
    }

    private void ListenEvents()
    {
        EventMediator.OnProviderToInterceptorAfterConsumed += InvokeAfterConsumed;
        EventMediator.OnProviderToInterceptorAfterPublished += InvokeAfterPublished;
        EventMediator.OnProviderToInterceptorError += InvokeError;
    }

    private async Task InvokeAfterConsumed(object data)
    {
        try
        {
            var newData = Convert.ChangeType(data, Interceptor.GetType().BaseType.GetGenericArguments()[0]);
            _context.DataInput = newData;

            var output = await Interceptor.InvokeMethodResultAsync("AfterConsumed", newData);
            if (output != null) _ = _eventWrapper.EmitOnInterceptorToProvider(output);
            _context.DataOutput = output;
        }
        catch (Exception ex)
        {
            var exception = new CoreException("Error calling AfterConsuming method!", ex);
            _context.Exceptions.Add(exception);
            _log.LogError(ex.ToString());

            _ = Interceptor.InvokeMethodAsync("Error", exception);
        }
    }

    private Task InvokeAfterPublished(Context data)
        => _ = Interceptor.InvokeMethodAsync("AfterPublished", data);

    private Task InvokeError(CoreException data)
        => Interceptor.InvokeMethodAsync("Error", data, _context);

}

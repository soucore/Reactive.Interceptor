using Reactive.Interceptor.Core.Interfaces;

namespace Reactive.Interceptor.Core;
public class EventMediator: IEventMediator
{
    public static event Func<Context, Task> OnProviderToInterceptorAfterPublished;
    public static event Func<CoreException, Task> OnProviderToInterceptorError;
    public static event Func<object, Task> OnProviderToInterceptorAfterConsumed;
    public static event Func<object, Task> OnInterceptorToProviderSink;
    public static event Action<bool> OnBreakProvider;

    public async Task EmitFromProviderToInterceptorAfterConsumed(object data)
        => await OnProviderToInterceptorAfterConsumed?.Invoke(data);

    public async Task EmitOnInterceptorToProvider(object data)
        => await OnInterceptorToProviderSink?.Invoke(data);

    public async Task EmitFromProviderToInterceptorAfterPublished(Context data)
        => await OnProviderToInterceptorAfterPublished?.Invoke(data);

    public async Task EmitFromProviderToInterceptorError(CoreException interceptorException)
        => await OnProviderToInterceptorError?.Invoke(interceptorException);

    public void EmitBreakToProvider(bool breaker = true)
        => OnBreakProvider?.Invoke(breaker);
}

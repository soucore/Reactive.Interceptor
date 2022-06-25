namespace Reactive.Interceptor.Core.Interfaces;
public interface IEventMediator
{
    Task EmitOnInterceptorToProvider(object data);
    Task EmitFromProviderToInterceptorAfterConsumed(object data);
    Task EmitFromProviderToInterceptorAfterPublished(Context data);
    Task EmitFromProviderToInterceptorError(CoreException interceptorException);
    void EmitBreakToProvider(bool breaker = true);
}

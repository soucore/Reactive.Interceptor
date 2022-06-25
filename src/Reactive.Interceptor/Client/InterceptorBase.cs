using Reactive.Interceptor.Core;

namespace Reactive.Interceptor.Client;
public abstract class InterceptorBase<TInput, TOutput>
{
    public abstract Task<TOutput> AfterConsumed(TInput data);
    public virtual Task AfterPublished(Context data)
    { return Task.CompletedTask; }
    public virtual Task Error(CoreException data, Context context)
    { return Task.CompletedTask; }
}

using Reactive.Interceptor.Core;

namespace Reactive.Interceptor.Providers.Interface;
public interface IProviderBase
{
    public event Action<object> EventSource;
    protected Task ExecuteSourceAsync(CancellationToken cancellationToken);
    protected Task<Response> Sink(object data);
    Task EmitBySource<T>(T data);
}

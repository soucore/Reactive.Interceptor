using Reactive.Interceptor.Core;
using Reactive.Interceptor.Providers.Interface;

namespace Reactive.Interceptor.Providers;
public abstract class ProviderBase : IProviderBase, IDisposable
{
    private Task _executeTask;
    private CancellationTokenSource _stoppingCts;
    protected bool IsBreak;

    public event Action<object> EventSource;
    public abstract string ProviderName { get; }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        EventMediator.OnBreakProvider += DefineSleep;
        _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1]
        {
                cancellationToken
        });
        _executeTask = ExecuteSourceAsync(_stoppingCts.Token);
        if (_executeTask.IsCompleted)
            return _executeTask;

        return Task.CompletedTask;
    }

    private void DefineSleep(bool isBreak)
        => IsBreak = isBreak;

    public abstract Task ExecuteSourceAsync(CancellationToken cancellationToken);
    public abstract Task<Response> Sink(object data);
    public async Task EmitBySource<T>(T data)
    {
        EventSource?.Invoke(data);
        await Task.CompletedTask;
    }
    public virtual void Dispose()
        => _stoppingCts?.Cancel();
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using Reactive.Interceptor.Core.Configurations;
using Reactive.Interceptor.Core.Interfaces;

namespace Reactive.Interceptor.Core;
public class PolicyStrategy : IPolicyStrategy
{
    public const string SleepName = "ls";

    private readonly IOptions<ConfigurationClient> _configurations;
    private readonly IEventMediator _event;
    private readonly ILogger<PolicyStrategy> _log;

    public static Action<bool> EventCircuit;

    public PolicyStrategy(IOptions<ConfigurationClient> configurations,
        IEventMediator eventMediator,
        ILogger<PolicyStrategy> logger)
    {
        _configurations = configurations;
        _event = eventMediator;
        _log = logger;
    }

    public AsyncRetryPolicy<Response> GetPolicyRetry()
    {
        return Polly.Policy
             .Handle<CoreException>()
             .Or<BrokenCircuitException>()
             .OrResult<Response>(resp =>
             {
                 return !resp.Published || resp.ExceptionCore is not null;
             })
             .WaitAndRetryAsync(_configurations.Value.Policy.Retry.Attempts,
                 sleepDurationProvider: (c, ctx) =>
                 {
                     if (ctx.ContainsKey(SleepName) && ctx[SleepName] != null)
                         return (TimeSpan)ctx[SleepName];

                     return TimeSpan.FromSeconds(_configurations.Value.Policy.Retry.Timeout);
                 },
                 onRetry: (resp, ts, i, ctx) =>
                 {
                     _log.LogDebug($"{resp.Exception.GetType().Name}: {resp.Exception.Message}");
                 });
    }

    public AsyncCircuitBreakerPolicy GetCircuitBreaker()
    {
        return Polly.Policy
            .Handle<CoreException>()
            .Or<BrokenCircuitException>()
            .CircuitBreakerAsync(_configurations.Value.Policy.CircuitBreaker.ExceptionsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(_configurations.Value.Policy.CircuitBreaker.Sleep),
                onBreak: (ex, ts, ctx) =>
                {
                    ctx[SleepName] = ts;
                    _event.EmitBreakToProvider();
                    _log.LogDebug("Circuit Open!");
                },
                onHalfOpen: () =>
                {
                    _log.LogDebug("Circuit HalfOpen!");
                },
                onReset: (ctx) => {
                    ctx[SleepName] = null;
                    _event.EmitBreakToProvider(false);
                    _log.LogDebug("Circuit Closed!");
                }
            );
    }

    public AsyncPolicyWrap<Response> BuildStrategy()
    {
        var retry = GetPolicyRetry();
        var circuitBreaker = GetCircuitBreaker();

        return retry.WrapAsync(circuitBreaker);
    }
}

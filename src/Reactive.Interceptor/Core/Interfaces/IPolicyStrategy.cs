using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace Reactive.Interceptor.Core.Interfaces;

public interface IPolicyStrategy
{
    AsyncCircuitBreakerPolicy GetCircuitBreaker();
    AsyncRetryPolicy<Response> GetPolicyRetry();
    AsyncPolicyWrap<Response> BuildStrategy();
}

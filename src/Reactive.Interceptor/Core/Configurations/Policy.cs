namespace Reactive.Interceptor.Core.Configurations;

public class Policy
{
    public Retry Retry { get; set; }
    public CircuitBreaker CircuitBreaker { get; set; }
}

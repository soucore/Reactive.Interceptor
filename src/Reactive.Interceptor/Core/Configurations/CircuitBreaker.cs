namespace Reactive.Interceptor.Core.Configurations;
public class CircuitBreaker
{
    public bool Disabled { get; set; }
    public int Sleep { get; set; }
    public int ExceptionsAllowedBeforeBreaking { get; set; }
}

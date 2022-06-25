namespace Reactive.Interceptor.Core.Configurations;

public class Retry
{
    public bool Disabled { get; set; }
    public int Attempts { get; set; }
    public int Timeout { get; set; }
}

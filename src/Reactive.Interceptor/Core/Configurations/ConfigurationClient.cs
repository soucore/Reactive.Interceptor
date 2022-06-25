namespace Reactive.Interceptor.Core.Configurations;

public class ConfigurationClient
{
    public static string Key = "ReactiveInterceptor";
    public Connector Source { get; set; }
    public Connector Sink { get; set; }
    public Connector DeadLetter { get; set; }
    public Policy Policy { get; set; }
}

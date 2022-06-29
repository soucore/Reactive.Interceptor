using Reactive.Interceptor.Extensions;
using Reactive.Interceptor.Provider.Example1;
using Reactive.Interceptor.Provider.Example2;
using Reactive.Interceptor.Provider.Example3;
using Serilog;
using Serilog.Events;
using Worker;

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddReactiveInterceptor<InterceptorAction>();
        services.AddReactiveProviderExample1();
        services.AddReactiveProviderExample2();
        services.AddReactiveProviderExample3();
    })
    .UseReactiveInterceptor()
    .Build();
    
await host.RunAsync();

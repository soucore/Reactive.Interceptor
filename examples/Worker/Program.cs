using Reactive.Interceptor;
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
    .UseReactiveInterceptor<InterceptorAction>()
    .UseReactiveProviderExample1()
    .UseReactiveProviderExample2()
    .UseReactiveProviderExample3()
    .Build();
    
await host.RunAsync();

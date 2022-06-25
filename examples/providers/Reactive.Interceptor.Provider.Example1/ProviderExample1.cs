using Microsoft.Extensions.Logging;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Core.Interfaces;
using Reactive.Interceptor.Providers;

namespace Reactive.Interceptor.Provider.Example1
{
    public class ProviderExample1 : ProviderBase
    {
        private readonly IConfigurationProvider<MyConfigurations> _configuration;
        private readonly ILogger<ProviderExample1> _log;

        public ProviderExample1(ILogger<ProviderExample1> logger,
            IConfigurationProvider<MyConfigurations> configuration)
        {
            _log = logger;
            _configuration = configuration;
        }

        public override string ProviderName => "example1";

        public override async Task ExecuteSourceAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (IsBreak) continue;

                Thread.Sleep(2000);

                var data = DateTime.Now.Second;
                _log.LogDebug("=============");
                _log.LogDebug($"PROVIDER[{ProviderName}](Source) ► Dado consumido! {data}");
  
                await EmitBySource(data);
            }
        }

        public override async Task<Response> Sink(object data)
        {
            try
            {
                _log.LogDebug($"PROVIDER[{ProviderName}](Sink) ► Dado enviado! {data}");

                return new Response("Data sent successfully!");
            }
            catch (Exception ex)
            {
                return new Response("Failed to send message!", ex);
            }
            finally
            {
                await Task.CompletedTask;
            }
        }
    }
}

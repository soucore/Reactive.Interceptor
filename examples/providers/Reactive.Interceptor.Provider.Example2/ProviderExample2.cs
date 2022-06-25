using Microsoft.Extensions.Logging;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Providers;

namespace Reactive.Interceptor.Provider.Example2
{
    public class ProviderExample2 : ProviderBase
    {
        private readonly ILogger<ProviderExample2> _log;

        public override string ProviderName => "example2";

        public ProviderExample2(ILogger<ProviderExample2> logger)
        {
            _log = logger;
        }

        public override Task ExecuteSourceAsync(CancellationToken cancellationToken)
        {
            _log.LogDebug($"PROVIDER[{ProviderName}](Source) ► ");

            return Task.CompletedTask;
        }

        public override async Task<Response> Sink(object data)
        {
            try
            {
                _log.LogDebug($"PROVIDER[{ProviderName}](Sink) ► {data}");
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

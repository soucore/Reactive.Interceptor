using Microsoft.Extensions.Logging;
using Reactive.Interceptor.Core;
using Reactive.Interceptor.Providers;

namespace Reactive.Interceptor.Provider.Example3
{
    public class ProviderExample3 : ProviderBase
    {
        private readonly ILogger<ProviderExample3> logger;

        public override string ProviderName => "example3";

        public ProviderExample3(ILogger<ProviderExample3> logger)
        {
            this.logger = logger;
        }

        public override async Task ExecuteSourceAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<Response> Sink(object data)
        {
            logger.LogDebug($"PROVIDER[{ProviderName}](Sink) ► {data}");

            return new Response("Sent to Dead Letter!");
        }
    }
}
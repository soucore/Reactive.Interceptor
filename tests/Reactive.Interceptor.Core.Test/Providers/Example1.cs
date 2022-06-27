using Reactive.Interceptor.Providers;
using System.Threading;
using System.Threading.Tasks;

namespace Reactive.Interceptor.Core.Test.Providers
{
    public class Example1 : ProviderBase
    {
        public override string ProviderName => throw new System.NotImplementedException();

        public override Task ExecuteSourceAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task<Response> Sink(object data)
        {
            throw new System.NotImplementedException();
        }
    }
}

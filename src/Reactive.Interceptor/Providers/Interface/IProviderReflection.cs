using Reactive.Interceptor.Providers.Interface;

namespace Reactive.Interceptor.Core.Interfaces;
public interface IProviderReflection
{
    Task Run(Type interceptorType, CancellationToken cancellationToken);
    IEnumerable<Type> DiscoverProviders();
    Task BuildType(Type type);
    Task BuildProviders(IEnumerable<Type> providerTypes);
    Task ExcecuteIfExistSource(IProviderBase provider, string providerName);
}

using Reactive.Interceptor.Providers.Interface;
using System.Reflection;

namespace Reactive.Interceptor.Core.Interfaces;
public interface IProviderReflection
{
    Task Run(Type interceptorType, CancellationToken cancellationToken);
    IEnumerable<Type> DiscoverProviders(IEnumerable<AssemblyName> assemblies);
    Task BuildType(Type type);
    Task BuildProviders(IEnumerable<Type> providerTypes);
    Task ExcecuteIfExistSource(IProviderBase provider, string providerName);
}

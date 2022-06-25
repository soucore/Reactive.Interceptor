using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Reactive.Interceptor.Core.Configurations;
using Reactive.Interceptor.Core.Enum;
using Reactive.Interceptor.Core.Interfaces;
using System.Collections.Concurrent;

namespace Reactive.Interceptor.Core;
public class ConfigurationProvider<T> : IConfigurationProvider<T>
{
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<SectionEnum, object> Cache;

    public ConfigurationClient ConfigurationClient { get; }
    public ConfigurationProvider(IConfiguration configuration,
        IOptions<ConfigurationClient> configuraitonClient)
    {
        _configuration = configuration;
        ConfigurationClient = configuraitonClient.Value;
        Cache = new ConcurrentDictionary<SectionEnum, object>();
    }

    public T GetBySource() => Get(SectionEnum.Source);

    public T GetBySink() => Get(SectionEnum.Sink);

    private T Get(SectionEnum section)
    {
        if (Cache.TryGetValue(section, out object value)) 
            return (T) value;

        return ConfigurationValue(section);
    }

    private T ConfigurationValue(SectionEnum section)
    {
        var session = $"{SectionEnum.ReactiveInterceptor}:{section}:{SectionEnum.Configurations}";
        var newValue = _configuration.GetSection(session).Get<T>();
        Cache.TryAdd(section, newValue);

        return newValue;
    }
}

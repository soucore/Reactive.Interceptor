using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Reactive.Interceptor.Core.Configurations;
using System.Collections.Generic;
using Xunit;

namespace Reactive.Interceptor.Core.Test
{
    public class ConfigurationProviderTest
    {

        [Fact]
        public void Get_By_Source_Configurations_Within_Value()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ReactiveInterceptor:Source:Configurations:Host", "localhost:9091" },
                {"ReactiveInterceptor:Source:Configurations:Test", "Test1" },
                {"ReactiveInterceptor:Sink:Configurations:Host", "localhost:9092" },
                {"ReactiveInterceptor:Sink:Configurations:Test", "Test2" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var sectionConfig = configuration.GetSection("ReactiveInterceptor").Get<ConfigurationClient>();
            var someOptions = Options.Create(sectionConfig);
            var exceptedSource = new ConfigTest { Host = "localhost:9091", Test = "Test1" };
            var exceptedSink = new ConfigTest { Host = "localhost:9092", Test = "Test2" };

            //Action
            var configurations = new ConfigurationProvider<ConfigTest>(configuration, someOptions);

            //Assert
            configurations.GetBySource().Should().BeEquivalentTo(exceptedSource);
            configurations.GetBySink().Should().BeEquivalentTo(exceptedSink);
        }
    }

    public class ConfigTest
    {
        public string Host { get; set; }
        public string Test { get; set; }
    }
}

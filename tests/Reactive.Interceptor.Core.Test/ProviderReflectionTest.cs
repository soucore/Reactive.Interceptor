using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reactive.Interceptor.Core.Configurations;
using Reactive.Interceptor.Core.Interfaces;
using Reactive.Interceptor.Core.Test.Providers;
using Reactive.Interceptor.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Reactive.Interceptor.Core.Test
{
    public class ProviderReflectionTest
    {
        private readonly Moq.Mock<ILogger<ProviderReflection>>  _mockLogger = new();
        private readonly Moq.Mock<IOptions<ConfigurationClient>>  _mockOptionsConfig = new();
        private readonly Moq.Mock<IEventMediator>  _mockEventWrap = new();
        private readonly Moq.Mock<IPolicyStrategy>  _mockPolicy = new();
        private readonly List<AssemblyName> _assemblies = new();

        private readonly IServiceProvider serviceProvider;

        public ProviderReflectionTest()
        {
            // Setup
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceProvider = services.BuildServiceProvider();

            _assemblies.Add(Assembly.GetExecutingAssembly()?.GetName());
        }

        [Fact]
        public void Discover_Providers()
        {
            // Arrange
            var provider = new ProviderReflection(_mockLogger.Object,
                            new Context(),
                            _mockOptionsConfig.Object,
                            serviceProvider,
                            _mockEventWrap.Object,
                            _mockPolicy.Object);

            //Action
            var result = provider.DiscoverProviders(_assemblies);

            //Assert
            Assert.True(result.Any());
            Assert.True(result.Count() == 1);
            result.Select(g => g.Should().As<Example1>()).Should().HaveCount(1);
        }
    }
}

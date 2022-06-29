using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Reactive.Interceptor.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReactiveInterceptor<T>(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();
            return new Bootstrap(services, configuration)
                .Inject<T>().Services;
        }

        public static IServiceCollection AddReactiveInterceptor<T>(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            return new Bootstrap(services, configuration)
                .Inject<T>().Services;
        }

        public static IServiceCollection AddReactiveInterceptor(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            return new Bootstrap(services, configuration)
                .Inject().Services;
        }

        public static IServiceCollection AddReactiveInterceptor(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            return new Bootstrap(services, configuration)
                .Inject().Services;
        }
    }
}

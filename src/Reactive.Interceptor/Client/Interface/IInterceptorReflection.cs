namespace Reactive.Interceptor.Client.Interface;
public interface IInterceptorReflection
{
    Task Run(Type type);
}

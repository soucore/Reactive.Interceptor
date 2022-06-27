using System.Reflection;

namespace Reactive.Interceptor.Core.Extensions
{
    public static class ObjectExtension
    {
        public static async Task<object> InvokeTaskMethodResultAsync(this object obj, string name, params object[] parameters)
        {
            MethodInfo method = obj.GetType().GetMethod(name);
            if (parameters is not null)
                return await method.InvokeTaskAsync(obj, parameters);

            return await method.InvokeTaskAsync(obj);
        }

        public static async Task InvokeMethodAsync(this object obj, string name, params object[] parameters)
        {
            MethodInfo method = obj.GetType().GetMethod(name);
            method.Invoke(obj, parameters);

            await Task.CompletedTask;
        }
    }
}

using System.Reflection;

namespace Reactive.Interceptor.Core.Extensions;

public static class ExtensionMethods
{
    public static async Task<object> InvokeTaskAsync(this MethodInfo @this, object obj, params object[] parameters)
    {
        var value = (Task)@this.Invoke(obj, parameters);
        await value.ConfigureAwait(false);
        var result = value.GetType().GetProperty("Result");

        return result.GetValue(value);
    }
}

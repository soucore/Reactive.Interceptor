namespace Reactive.Interceptor.Core.Helpers;

public static class ConvertHelper
{
    public static bool TryChangeType<T>(object value, out T output)
    {
        output = default;

        try
        {
            output = (T)Convert.ChangeType(value, typeof(T));
            return true;
        }
        catch
        {
            return false;
        }
    }
}

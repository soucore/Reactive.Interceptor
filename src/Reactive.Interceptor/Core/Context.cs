namespace Reactive.Interceptor.Core;
public class Context
{
    public Context() => Exceptions = new List<CoreException>();

    public object DataInput { get; set; }
    public object DataOutput { get; set; }
    public int Retry { get; set; }
    public Response Response { get; set; }
    public IList<CoreException> Exceptions { get; set; }
    public object DataInputOriginal { get; internal set; }
    public static Context Clear() => new();
}

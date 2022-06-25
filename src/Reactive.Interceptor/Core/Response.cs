namespace Reactive.Interceptor.Core
{
    public class Response
    {
        public Response(Exception exception)
            :this(null, exception) { }
        public Response(string message, Exception exceptionCore)
        {
            Message = message;
            ExceptionCore = exceptionCore;
        }
        public Response(string message, bool published = true)
        {
            Message = message;
            Published = published;
        }
        
        public string Message { get; set; }
        public bool Published { get; set; }
        public Exception ExceptionCore { get; set; }
    }
}

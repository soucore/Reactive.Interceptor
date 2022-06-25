using Reactive.Interceptor.Client;
using Reactive.Interceptor.Core;

namespace Worker
{
    public class InterceptorAction : InterceptorBase<int, Person>
    {
        private readonly ILogger<InterceptorAction> logger;

        public InterceptorAction(ILogger<InterceptorAction> logger)
        {
            logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} - InterceptorAction On!");
            this.logger = logger;
        }

        public override Task<Person> AfterConsumed(int data)
        {
            logger.LogInformation($"INTERCEPTOR ► Dado recebido interceptado! {data}");
            return Task.FromResult(new Person(1, "Bruno", data));
        }

        public override async Task AfterPublished(Context data)
        {
            //Thread.Sleep(10000);

            if (data.Exceptions.Count() > 0)
            {
                logger.LogError($"INTERCEPTOR ► {data.Exceptions.Last()?.Message}");
                await Task.CompletedTask;
            }

            logger.LogInformation($"INTERCEPTOR ► {data.Response.Message}");
            await Task.CompletedTask;
        }

        public override Task Error(CoreException data, Context context)
        {
            logger.LogError($"INTERCEPTOR (Error) ► {data.Message}");

            return base.Error(data, context);
        }
    }
}

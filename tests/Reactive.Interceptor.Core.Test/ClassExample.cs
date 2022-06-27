using System.Threading.Tasks;

namespace Reactive.Interceptor.Core.Test
{
    public class ClassExample
    {
        public int Value { get; set; }

        public void Method1()
        {
            
        }

        public int Method2()
        {
            return Value;
        }

        public Task<int> MethodTask1()
        {
            return Task.FromResult(Value);
        }

        public Task<int> MethodTask2(int v)
        {
            return Task.FromResult(Value+v);
        }


    }
}

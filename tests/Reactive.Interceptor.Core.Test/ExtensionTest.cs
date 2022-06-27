using FluentAssertions;
using Reactive.Interceptor.Core.Extensions;
using System;
using Xunit;

namespace Reactive.Interceptor.Core.Test
{
    public class ExtensionTest
    {

        [Fact]
        public void Invoke_Task_Method_Existent_Without_Param()
        {
            //Arrange
            ClassExample @class = new();
            var expected = @class.Value = 2;
            Type type = @class.GetType();

            //Action
            var value = type.GetMethod("MethodTask1").InvokeTaskAsync(@class).Result;
            var value2 = () => type.GetMethod("Method2").InvokeTaskAsync(@class).Result;

            //Assert
            Assert.Equal(expected, value);
            value2.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void Invoke_Task_Method_Existent_Within_Param()
        {
            //Arrange
            ClassExample @class = new();
            var expected = @class.Value = 2;
            expected += 3;

            Type type = @class.GetType();

            //Action
            var value = type.GetMethod("MethodTask2").InvokeTaskAsync(@class, new object[] {3}).Result;
            var value2 = () => type.GetMethod("MethodTask1").InvokeTaskAsync(@class, new object[] {3}).Result;

            //Assert
            Assert.Equal(expected, value);
            value2.Should()
                .Throw<AggregateException>()
                .WithMessage("One or more errors occurred. (Parameter count mismatch.)");

        }

        [Fact]
        public void Invoke_Task_Method_ResultAsync_Existent_Whitout_Params()
        {
            //Arrange
            ClassExample @class = new();
            var expected = @class.Value;
            Type type = @class.GetType();

            //Action
            var value = @class.InvokeTaskMethodResultAsync("MethodTask1").Result;
            var value2 = () => @class.InvokeTaskMethodResultAsync("Method2").Result;

            //Assert
            Assert.Equal(expected, value);
            value2.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void Invoke_Task_Method_ResultAsync_Existent_Whitin_Params()
        {
            //Arrange
            ClassExample @class = new();
            var expected = @class.Value = 2;
            expected += 4;
            Type type = @class.GetType();

            //Action
            var value = @class.InvokeTaskMethodResultAsync("MethodTask2", new object[] {4}).Result;
            var value2 = () => @class.InvokeTaskMethodResultAsync("MethodTask1", new object[] { 4 }).Result;

            //Assert
            Assert.Equal(expected, value);
            value2.Should().Throw<AggregateException>();
        }
    }
}

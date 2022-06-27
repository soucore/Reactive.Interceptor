using System.Collections.Generic;
using Xunit;
using static Reactive.Interceptor.Core.Helpers.ConvertHelper;

namespace Reactive.Interceptor.Core.Test;
public class HelperConvertTest
{
    [Fact]
    public void TryChangeTypeIsTrue()
    {
        // Arrange
        const int value = 2;
        const string expected = "2";

        // Act
        var isChange = TryChangeType(value, out string output);

        // Assert
        Assert.True(isChange);
        Assert.True(output == expected);
    }

    [Fact]
    public void TryChangeTypeIsFalse()
    {
        // Arrange
        List<int> value = new();
        const int expected = 0;

        // Act
        var isChange = TryChangeType(value, out int output);

        // Assert
        Assert.False(isChange);
        Assert.True(output == expected);
    }
}
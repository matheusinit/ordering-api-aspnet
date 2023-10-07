namespace OrderingApi.UnitTest;

using OrderingApi.Domain;
using Xunit;

public class OrderUnitTesting
{
    [Fact]
    public void WhenProductIsNotProvidedThenShouldThrow()
    {
        var exception = Record.Exception(() => new Order(_product: null));

        Assert.IsType<ArgumentException>(exception);
    }
}

namespace OrderingApi;

using Xunit;

public class ProductUnitTesting
{
    [Fact]
    public void WhenNameIsNotProvidedShouldThrow()
    {
        var exception = Record.Exception(() => new Product(_name: null));

        Assert.IsType<ArgumentException>(exception);
    }
}

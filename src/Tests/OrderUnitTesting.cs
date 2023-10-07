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

    [Fact]
    public void WhenInstantiatedWithProductThenShouldAssignAnId()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _description: null);

        var order = new Order(_product: product);

        var regex = @"[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}";
        Assert.Matches(regex, order.Id);
    }

    [Fact]
    public void WhenInstantiatedWithProductThenShouldDefineCreatedAt()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _description: null);

        var order = new Order(_product: product);

        Assert.IsType<DateTime>(order.CreatedAt);
    }
}

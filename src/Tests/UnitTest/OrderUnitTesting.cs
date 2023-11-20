namespace OrderingApi.UnitTest;

using System;
using OrderingApi.Domain;
using Xunit;

public class OrderUnitTesting
{
    [Fact]
    public void WhenProductIsNotProvidedThenShouldThrow()
    {
        var exception = Record.Exception(() => new Order(null));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void WhenInstantiatedWithProductThenShouldAssignAnId()
    {
        var productId = Guid.NewGuid().ToString();

        var order = new Order(productId);

        var regex = @"[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}";
        Assert.Matches(regex, order.Id);
    }

    [Fact]
    public void WhenInstantiatedWithProductThenShouldDefineCreatedAt()
    {
        var productId = Guid.NewGuid().ToString();

        var order = new Order(productId);

        Assert.NotNull<DateTime>(order.CreatedAt);
    }

    [Fact]
    public void WhenOrderIsCanceledThenShouldDefineCanceledAt()
    {
        var productId = Guid.NewGuid().ToString();

        var order = new Order(productId);

        order.Cancel();

        Assert.NotNull<DateTime>(order.CanceledAt);
    }
}

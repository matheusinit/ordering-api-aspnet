namespace OrderingApi.UnitTest;

using Xunit;
using OrderingApi.Data;
using Microsoft.EntityFrameworkCore;
using OrderingApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using OrderingApi.IntegrationTest;
using OrderingApi.Producers;

public class FakeApplicationContext : ApplicationContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        throw new Exception("An inner error occurred");
    }
}

public class FakeOrderingProducer : OrderingProducer
{
    public Task<bool> SendOrderThroughMessageQueue(string topic, OrderToProduce order)
    {
        throw new Exception("An inner error occurred");
    }
}

public class OrderProductControllerUnitTesting
{
    [Fact]
    public async void WhenErrorIsThrownThenShouldReturnInternalServerError()
    {
        var fakeApplicationContext = new FakeApplicationContext();
        var fakeOrderingProducer = new FakeOrderingProducer();
        var sut = new OrderProductController(
            context: fakeApplicationContext,
            producer: fakeOrderingProducer
        );

        var response = await sut.Order(
            new OrderProductRequest { productId = Guid.NewGuid().ToString() }
        );

        var result = response.Result.As<ObjectResult>();
        var value = result.Value as ResponseError;
        result.Should().BeOfType<ObjectResult>();
        result.StatusCode.Should().Be(500);
        value?.message.Should().Be("An inner error occurred. Try again later.");
    }
}

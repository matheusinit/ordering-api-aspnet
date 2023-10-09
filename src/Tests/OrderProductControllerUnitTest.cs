namespace OrderingApi.UnitTest;

using Xunit;
using OrderingApi.Data;
using Microsoft.EntityFrameworkCore;
using OrderingApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using OrderingApi.IntegrationTest;
using System.Text;

public class FakeApplicationContext : ApplicationContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        throw new Exception("An inner error occurred");
    }
}

public class OrderProductControllerUnitTesting
{
    [Fact]
    public void WhenErrorIsThrownThenShouldReturnInternalServerError()
    {
        var fakeApplicationContext = new FakeApplicationContext();
        var sut = new OrderProductController(context: fakeApplicationContext);

        var response = sut.Order(new OrderProductRequest { productId = Guid.NewGuid().ToString() });

        var result = response.Result.As<ObjectResult>();
        var value = result.Value as ResponseError;
        result.Should().BeOfType<ObjectResult>();
        result.StatusCode.Should().Be(500);
        value?.message.Should().Be("An inner error occurred. Try again later.");
    }
}

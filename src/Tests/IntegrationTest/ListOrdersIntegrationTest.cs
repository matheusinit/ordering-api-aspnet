namespace OrderingApi.IntegrationTest;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using OrderingApi.Data;
using OrderingApi.Domain;
using Xunit;

public class ListOrdersIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public ListOrdersIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    [Fact]
    public async Task WhenNoOrdersExistThenShouldGetEmptyList()
    {
        var context = new ApplicationContext();

        context.Orders.ExecuteDelete<Order>();

        var response = await _client.GetAsync("/orders");
        var list = await response.Content.ReadFromJsonAsync<List<OrderView>>();

        list.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenThereIsOrdersStoredThenShouldGetListWithOrders()
    {
        var context = new ApplicationContext();
        var productId = Guid.NewGuid().ToString();
        var order = new Domain.Order(productId);
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync("/orders");
        var list = await response.Content.ReadFromJsonAsync<List<OrderView>>();

        list.Should().NotBeEmpty();
    }
}

namespace OrderingApi.IntegrationTest;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.Data;
using OrderingApi.Domain;
using Xunit;

public class CancelOrderIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;
    private ApplicationContext _context;

    public CancelOrderIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();

        _context = new ApplicationContext();
    }

    [Fact]
    public async Task WhenInvalidIdIsProvidedThenShouldGetBadRequest()
    {
        var invalidId = 1;
        var httpResponse = await _client.PatchAsync($"/orders/{invalidId}", null);

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task WhenIdOfNonExistingOrderIsProvidedThenShouldGetNotFound()
    {
        var id = Guid.NewGuid().ToString();
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task WhenIdOfCanceledOrderIsProvidedThenShouldGetBadRequest()
    {
        var product = AddProductToDb();
        var order = CreateOrder(product);
        order.Cancel();
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<ResponseError>();

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
        responseBody?.message.Should().Be("Order is already canceled");
    }

    private Product AddProductToDb()
    {
        var product = new Product(_name: "Product 1", _price: 100, _description: "Description 1");
        _context.Products.Add(product);

        return product;
    }

    private Order CreateOrder(Product product)
    {
        var order = new Order(product);

        return order;
    }

    private void AddOrder(Order order)
    {
        _context.Orders.Add(order);
    }

    private async Task SaveInDb()
    {
        await _context.SaveChangesAsync();
    }
}

namespace OrderingApi.IntegrationTest;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.Data;
using Xunit;
using FluentAssertions;
using OrderingApi.Producers;

class ResponseError
{
    public string? message { get; set; }
}

public enum OrderStatus
{
    NotSent
}

public class OrderProductResponseBody
{
    public string id { get; set; }
    public string status { get; set; }
    public string productId { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? canceledAt { get; set; }
}

public class FakeOrderingProducer : OrderingProducer
{
    public Task<bool> SendOrderThroughMessageQueue(string topic, OrderToProduce order)
    {
        return Task.FromResult(true);
    }
}

[Collection("Sequential")]
public class OrderProductIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;
    private readonly ApplicationContext _context;

    public OrderProductIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("..");
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<OrderingProducer, FakeOrderingProducer>();
                });
            })
            .CreateClient();
        _context = new ApplicationContext();
    }

    [Fact]
    public async Task WhenProductIdIsNotProvidedThenShouldGetBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/orders", new { });

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(responseBody?.message, "Product id is required");
    }

    [Fact]
    public async Task WhenValidInputIsProvidedThenShouldStoreOrderInDatabase()
    {
        var productId = Guid.NewGuid();
        var response = await _client.PostAsJsonAsync("/orders", new { productId = productId });
        var responseBody = await response.Content.ReadFromJsonAsync<OrderProductResponseBody>();

        var order = await _context.Orders.FindAsync(responseBody?.id);

        order.Should().NotBeNull();
    }
}

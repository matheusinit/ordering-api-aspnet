namespace OrderingApi.IntegrationTest;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.Data;
using OrderingApi.Domain;
using Xunit;

public class OrderProductResponseBody
{
    public string id { get; set; }
    public string status { get; set; }
    public string productId { get; set; }
    public string createdAt { get; set; }
    public string updatedAt { get; set; }
    public string deletedAt { get; set; }
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
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
        _context = new ApplicationContext();
    }

    [Fact]
    public async Task WhenProductIdIsNotProvidedThenShouldGetBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/order", new { });

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(responseBody?.message, "Product id is required");
    }

    [Fact]
    public async Task WhenProductIdDoesNotExistThenShouldGetNotFound()
    {
        var response = await _client.PostAsJsonAsync(
            "/order",
            new { productId = Guid.NewGuid().ToString() }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(responseBody?.message, "Product not found");
    }

    [Fact]
    public async Task WhenProductIdExistsThenShouldGetOk()
    {
        var product = new Product(
            _name: "Product 1",
            _price: 100,
            _description: "Description 1",
            _id: Guid.NewGuid().ToString()
        );
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var response = await _client.PostAsJsonAsync("/order", new { productId = product.Id });

        var responseBody = await response.Content.ReadFromJsonAsync<OrderProductResponseBody>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(product.Id, responseBody?.productId);
    }
}
namespace OrderingApi.IntegrationTest;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Bogus;
using System.Net.Http.Json;
using OrderingApi.Data;
using OrderingApi.Controllers;
using Microsoft.EntityFrameworkCore;

class ResponseError
{
    public string? message { get; set; }
}

class ProductResponseBody
{
    public required string id { get; set; }
    public required string name { get; set; }
    public decimal price { get; set; }
    public string? description { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? deletedAt { get; set; }
}

[Collection("Sequential")]
public class CreateProductIntegrationTest
    : IDisposable,
        IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public CreateProductIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    public void Dispose()
    {
        var context = new ApplicationContext();
        context.Products.ExecuteDelete<Domain.Product>();
    }

    [Fact]
    public async Task WhenNameIsNotProvidedThenShouldGetBadRequest()
    {
        var sut = await _client.PostAsJsonAsync("/products", new { name = "" });

        var responseBody = await sut.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
        Assert.Equal(responseBody?.message, "Name is required");
    }

    [Fact]
    public async Task WhenPriceIsNotProvidedThenShouldGetBadRequest()
    {
        var randomProductName = new Faker().Commerce.ProductName();

        var sut = await _client.PostAsJsonAsync("/products", new { name = randomProductName, });

        var responseBody = await sut.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
        Assert.Equal(responseBody?.message, "Price is required");
    }

    [Fact]
    public async Task WhenPriceProvidedIsLessThanZeroThenShouldGetBadRequest()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(-999999, -1);

        var sut = await _client.PostAsJsonAsync(
            "/products",
            new { name = randomProductName, price = randomPrice }
        );

        var responseBody = await sut.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
        Assert.Equal(responseBody?.message, "Price cannot be less than zero");
    }

    [Fact]
    public async Task WhenDescriptionIsNotProvidedThenShouldGetCreated()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);

        var sut = await _client.PostAsJsonAsync(
            "/products",
            new { name = randomProductName, price = randomPrice }
        );

        var responseBody = await sut.Content.ReadFromJsonAsync<ProductResponseBody>();

        var expected = new
        {
            name = randomProductName,
            price = randomPrice,
            description = (string?)null
        };

        Assert.Equal(HttpStatusCode.Created, sut.StatusCode);
        Assert.Equal(expected.name, responseBody?.name);
        Assert.Equal(expected.price, responseBody?.price);
        Assert.Equal(expected.description, responseBody?.description);
        Assert.IsType<DateTime>(responseBody?.createdAt);
    }

    [Fact]
    public async Task WhenDescriptionIsProvidedThenShouldGetCreated()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var randomDescription = new Faker().Lorem.Sentence();

        var sut = await _client.PostAsJsonAsync(
            "/products",
            new
            {
                name = randomProductName,
                price = randomPrice,
                description = randomDescription
            }
        );

        var responseBody = await sut.Content.ReadFromJsonAsync<ProductResponseBody>();
        var expected = new
        {
            name = randomProductName,
            price = randomPrice,
            description = randomDescription
        };

        Assert.Equal(HttpStatusCode.Created, sut.StatusCode);
        Assert.Equal(expected.name, responseBody?.name);
        Assert.Equal(expected.price, responseBody?.price);
        Assert.Equal(expected.description, responseBody?.description);
        Assert.IsType<DateTime>(responseBody?.createdAt);
    }

    [Fact]
    public async Task WhenValidDataIsProvidedThenShouldStoreEntityInDatabase()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var randomDescription = new Faker().Lorem.Sentence();
        var response = await _client.PostAsJsonAsync(
            "/products",
            new
            {
                name = randomProductName,
                price = randomPrice,
                description = randomDescription
            }
        );
        var product = await response.Content.ReadFromJsonAsync<ProductResponseBody>();
        var context = new ApplicationContext();

        var productStoredInDb = await context.Products.FindAsync(product?.id);

        Assert.Equal(randomProductName, productStoredInDb?.Name);
        Assert.Equal(randomPrice, productStoredInDb?.Price);
        Assert.Equal(randomDescription, productStoredInDb?.Description);
    }

    [Fact]
    public async Task WhenValidDataIsProvidedShouldReturnWithLocation()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var randomDescription = new Faker().Lorem.Sentence();
        var requestBody = new ProductRequestBody
        {
            name = randomProductName,
            price = randomPrice,
            description = randomDescription
        };

        var response = await _client.PostAsJsonAsync("/products", requestBody);

        var regex = @"\/products\/[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}";
        Assert.Matches(regex, response.Headers.Location?.ToString());
    }
}

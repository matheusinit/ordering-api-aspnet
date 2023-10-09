namespace OrderingApi.IntegrationTest;

using System.Net;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using OrderingApi.Data;
using OrderingApi.View;
using Xunit;

class ProductChanges
{
    public string? name { get; set; }
    public int? price { get; set; }
    public string? description { get; set; }
}

class UpdateResponseError
{
    public required string message { get; set; }
}

[Collection("Sequential")]
public class UpdateProductIntegrationTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public UpdateProductIntegrationTesting()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();

        var context = new ApplicationContext();

        var products = context.Products.ToList();
        products.Select(p => context.Products.Remove(p));
    }

    private async Task<ProductView?> PostProduct()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var randomDescription = new Faker().Lorem.Sentence();
        var insertionData = new
        {
            name = randomProductName,
            price = randomPrice,
            description = randomDescription
        };
        var response = await _client.PostAsJsonAsync("/products", insertionData);
        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();
        return responseBody;
    }

    [Fact]
    public async Task WhenIdOfNonExistentProductIsProvidedThenShouldGetNotFound()
    {
        var invalidId = 1;
        var randomProductName = new Faker().Commerce.ProductName();
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{invalidId}",
            new ProductChanges { name = randomProductName }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<UpdateResponseError>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Product not found", responseBody?.message);
    }

    [Fact]
    public async Task WhenEmptyStringIsProvidedAsNameThenShouldGetBadRequest()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { name = "" }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task WhenIdOfExistingProductAndNameIsProvidedThenShouldGetOk()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomProductName = new Faker().Commerce.ProductName();
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { name = randomProductName }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(randomProductName, responseBody?.name);
    }

    [Fact]
    public async Task WhenIdOfExistingProductAndPriceIsProvidedThenShouldGetOk()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomPrice = new Faker().Random.Int(0, 999999);
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { price = randomPrice }
        );
        var randomPriceInDecimal = Decimal.Divide(randomPrice, 100.0m);

        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(randomPriceInDecimal, responseBody?.price);
    }

    [Fact]
    public async Task WhenValidInputIsProvidedToOnlyUpdateNameThenShouldReturnOnlyTheNameUpdated()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomProductName = new Faker().Commerce.ProductName();
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { name = randomProductName }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();

        Assert.Equal(randomProductName, responseBody?.name);
        Assert.Equal(productCreated?.price, responseBody?.price);
        Assert.Equal(productCreated?.description, responseBody?.description);
        Assert.Equal(productCreated?.createdAt, responseBody?.createdAt);
        Assert.Equal(productCreated?.deletedAt, responseBody?.deletedAt);
    }

    [Fact]
    public async Task WhenValidInputIsProvidedThenShouldReturnUpdatedAtWithDateTimeValue()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomProductName = new Faker().Commerce.ProductName();
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { name = randomProductName }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();

        Assert.NotNull(responseBody?.updatedAt);
        Assert.IsType<DateTime>(responseBody?.updatedAt);
    }

    [Fact]
    public async Task WhenEmptyStringIsProvidedAsDescriptionThenShouldGetBadRequest()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { description = "" }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task WhenIdOfExistingProductAndDescriptionIsProvidedThenShouldGetOk()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomDescription = new Faker().Lorem.Sentence();
        var response = await _client.PutAsJsonAsync<ProductChanges>(
            $"/products/{id}",
            new ProductChanges { description = randomDescription }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ProductView>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(randomDescription, responseBody?.description);
    }

    [Fact]
    public async Task WhenValidInputIsProvidedThenShouldProductBeUpdatedInDatabase()
    {
        var productCreated = await PostProduct();
        var id = productCreated?.id;
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var randomDescription = new Faker().Lorem.Sentence();
        var update = new ProductChanges
        {
            name = randomProductName,
            price = randomPrice,
            description = randomDescription
        };
        await _client.PutAsJsonAsync<ProductChanges>($"/products/{id}", update);
        var context = new ApplicationContext();

        var updatedProduct = await context.Products.FindAsync(id);

        Assert.Equal(randomProductName, updatedProduct?.Name);
        Assert.Equal(randomPrice, updatedProduct?.Price);
        Assert.Equal(randomDescription, updatedProduct?.Description);
    }
}

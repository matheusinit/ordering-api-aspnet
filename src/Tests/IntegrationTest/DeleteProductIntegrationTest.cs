using System.Net;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.Data;
using OrderingApi.View;
using Xunit;

namespace OrderingApi.IntegrationTest;

[Collection("Sequential")]
public class DeleteProductIntegrationTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public DeleteProductIntegrationTesting()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();

        var context = new ApplicationContext();
        var products = context.Products.ToList();
        products.Select(p => context.Products.Remove(p));
    }

    [Fact]
    public async Task WhenIdOfNonExistentProductIsProvidedThenShouldGetNotFound()
    {
        var invalidId = Guid.NewGuid().ToString();
        var response = await _client.DeleteAsync($"/products/{invalidId}");

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Product not found", responseBody?.message);
    }

    private async Task<ProductView?> createProduct()
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
        var responseCreation = await _client.PostAsJsonAsync("/products", insertionData);
        var responseBodyCreation = await responseCreation.Content.ReadFromJsonAsync<ProductView>();
        return responseBodyCreation;
    }

    [Fact]
    public async Task WhenValidIdIsProvidedThenShouldGetNoContent()
    {
        var product = await createProduct();
        var id = product?.id;

        var response = await _client.DeleteAsync($"/products/{id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task WhenValidIdIsProvidedThenShouldSoftDeleteProductFromDatabase()
    {
        var productCreated = await createProduct();
        var id = productCreated?.id;
        var response = await _client.DeleteAsync($"/products/{id}");
        var context = new ApplicationContext();

        var product = await context.Products.FindAsync(id);

        Assert.NotNull<DateTime>(product?.DeletedAt);
    }
}
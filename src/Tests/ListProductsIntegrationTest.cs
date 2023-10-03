namespace OrderingApi.IntegrationTest;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Bogus;
using OrderingApi.Data;
using Microsoft.EntityFrameworkCore;

class Product
{
    public required string id { get; set; }
    public required string name { get; set; }
    public double price { get; set; }
    public string? description { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? deletedAt { get; set; }
}

public class ListProductsIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public ListProductsIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();

        var context = new ApplicationContext();
        context.Products.ExecuteDelete<Domain.Product>();
    }

    [Fact]
    public async Task WhenNoProductsExistThenShouldGetEmptyList()
    {
        var list = await _client.GetFromJsonAsync<List<Product>>("/products");

        Assert.Empty(list);
    }

    [Fact]
    public async Task WhenThereIsProductsStoredThenShouldGetListWithProducts()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var response = await _client.PostAsJsonAsync(
            "/products",
            new { name = randomProductName, price = randomPrice }
        );
        var product = await response.Content.ReadFromJsonAsync<Product>();

        var list = await _client.GetFromJsonAsync<List<Product>>("/products");

        var productPriceInDouble = product?.price / 100;
        Assert.NotEmpty(list);
        Assert.Equal(product?.name, list.Find(p => p.id == product?.id).name);
        Assert.Equal(productPriceInDouble, list.Find(p => p.id == product?.id).price);
    }

    [Fact]
    public async Task WhenThereIsProductsStoredThenShouldGetProductPriceInDouble()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var response = await _client.PostAsJsonAsync(
            "/products",
            new { name = randomProductName, price = randomPrice }
        );
        var product = await response.Content.ReadFromJsonAsync<Product>();

        var list = await _client.GetFromJsonAsync<List<Product>>("/products");

        var productPriceInDouble = product?.price / 100;
        Assert.Equal(productPriceInDouble, list[0].price);
    }
}

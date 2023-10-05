namespace OrderingApi.IntegrationTest;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Bogus;
using OrderingApi.Data;
using Microsoft.EntityFrameworkCore;
using OrderingApi.View;

[Collection("Sequential")]
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
        var list = await _client.GetFromJsonAsync<List<ProductView>>("/products");

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
        var product = await response.Content.ReadFromJsonAsync<ProductView>();

        var list = await _client.GetFromJsonAsync<List<ProductView>>("/products");

        Assert.NotEmpty(list);
        Assert.Equal(product?.name, list.Find(p => p.id == product?.id).name);
        Assert.Equal(product?.price, list.Find(p => p.id == product?.id).price);
    }

    [Fact]
    public async Task WhenThereIsProductsStoredThenShouldGetProductPriceInDecimal()
    {
        var randomProductName = new Faker().Commerce.ProductName();
        var randomPrice = new Faker().Random.Int(0, 999999);
        var response = await _client.PostAsJsonAsync(
            "/products",
            new { name = randomProductName, price = randomPrice }
        );
        var product = await response.Content.ReadFromJsonAsync<ProductView>();

        var list = await _client.GetFromJsonAsync<List<ProductView>>("/products");

        var priceFound = list?.Find(p => p.id == product?.id)?.price ?? 0.00m;
        Assert.Equal(product?.price, priceFound);
    }
}

namespace OrderingApi.IntegrationTest;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

class Product
{
    public required string id { get; set; }
    public required string name { get; set; }
    public int price { get; set; }
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
    }

    [Fact]
    public async Task WhenNoProductsExistThenShouldGetEmptyList()
    {
        var sut = await _client.GetFromJsonAsync<List<Product>>("/products");

        Assert.Empty(sut);
    }
}

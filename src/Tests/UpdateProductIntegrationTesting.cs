namespace OrderingApi.IntegrationTest;

using System.Net;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

class ProductChanges
{
    public string? name { get; set; }
    public int? price { get; set; }
    public string? description { get; set; }
}

class UpdateResponseError
{
    public string message { get; set; }
}

public class UpdateProductIntegrationTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public UpdateProductIntegrationTesting()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
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
}

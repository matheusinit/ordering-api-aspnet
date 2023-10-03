using System.Net;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace OrderingApi.IntegrationTest;

public class DeleteProductIntegrationTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public DeleteProductIntegrationTesting()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    [Fact]
    public async Task WhenIdOfNonExistentProductIsProvidedThenShouldGetNotFound()
    {
        var invalidId = 1;
        var response = await _client.DeleteAsync($"/products/{invalidId}");

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Product not found", responseBody?.message);
    }
}

namespace OrderingApi.IntegrationTest;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

public class CancelOrderIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public CancelOrderIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    [Fact]
    public async Task WhenInvalidIdIsProvidedThenShouldGetBadRequest()
    {
        var invalidId = 1;
        var httpResponse = await _client.PatchAsync($"/orders/{invalidId}", null);

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task WhenIdOfNonExistingOrderIsProvidedThenShouldGetNotFound()
    {
        var id = Guid.NewGuid().ToString();
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status404NotFound);
    }
}

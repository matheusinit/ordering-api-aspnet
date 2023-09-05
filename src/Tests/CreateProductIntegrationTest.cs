namespace OrderingApi;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Microsoft.AspNetCore.TestHost;

public class CreateProductIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public CreateProductIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    [Fact]
    public async Task WhenNameIsNotProvidedThenShouldGetBadRequest()
    {
        var product = new KeyValuePair<string, string>("name", "");
        var sut = await _client.PostAsJsonAsync("/products", product);

        Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
    }
}

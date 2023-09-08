namespace OrderingApi;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Bogus;
using System.Net.Http.Json;

class ResponseError
{
    public string message { get; set; }
}

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
        var sut = await _client.PostAsJsonAsync("/products", new { product = "" });

        Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
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
}

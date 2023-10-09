namespace OrderingApi.IntegrationTest;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

public class ListOrdersIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;

    public ListOrdersIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(".."))
            .CreateClient();
    }

    [Fact]
    public async Task WhenNoOrdersExistThenShouldGetEmptyList()
    {
        var response = await _client.GetAsync("/orders");
        var list = await response.Content.ReadFromJsonAsync<List<OrderView>>();

        list.Should().BeEmpty();
    }
}

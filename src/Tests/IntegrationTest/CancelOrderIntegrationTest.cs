namespace OrderingApi.IntegrationTest;

using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.BackgroundServices;
using OrderingApi.Data;
using OrderingApi.Domain;
using Xunit;

[Collection("Sequential")]
public class CancelOrderIntegrationTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;
    private ApplicationContext _context;

    public CancelOrderIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("..");
                builder.ConfigureTestServices(services =>
                {
                    var serviceDescriptor = services.Single(
                        descriptor =>
                            descriptor.ServiceType == typeof(IHostedService)
                            && descriptor.ImplementationType
                                == typeof(StockConsumerBackgroundService)
                    );
                    services.Remove(serviceDescriptor);
                });
            })
            .CreateClient();

        _context = new ApplicationContext();

        DeleteAllOrderRecords();
    }

    public void Dispose()
    {
        DeleteAllOrderRecords();
    }

    private void DeleteAllOrderRecords()
    {
        var orders = _context.Orders.ToList();
        orders.Select(p => _context.Orders.Remove(p));
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

    [Fact]
    public async Task WhenIdOfCanceledOrderIsProvidedThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid().ToString();
        var order = CreateOrder(productId);
        order.Cancel();
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<ResponseError>();

        httpResponse.StatusCode
            .Should()
            .Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
        responseBody?.message.Should().Be("Order is already canceled");
    }

    [Fact]
    public async Task WhenValidIdIsProvidedThenShouldGetOk()
    {
        var productId = Guid.NewGuid().ToString();
        var order = CreateOrder(productId);
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<OrderView>();

        httpResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
        responseBody?.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenOrderHasMoreThan24HoursSinceCreationThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid().ToString();
        var order = CreateOrder(productId);
        order.CreatedAt = DateTime.Now.AddHours(-24).AddMinutes(-1);
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<ResponseError>();

        responseBody?.message
            .Should()
            .Be("Order cannot be canceled. Order was made more than 24 hours ago");
    }

    [Fact]
    public async Task WhenOrderIsCanceledThenShouldReturnStatusAsCanceled()
    {
        var productId = Guid.NewGuid().ToString();
        var order = CreateOrder(productId);
        order.CreatedAt = DateTime.Now.AddHours(-23);
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<OrderView>();

        responseBody?.status.Should().Be("Canceled");
    }

    [Fact]
    public async Task WhenValidIdIsProvidedThenShouldReturnCanceledDateTimeDefined()
    {
        var productId = Guid.NewGuid().ToString();
        var order = CreateOrder(productId);
        AddOrder(order);
        await SaveInDb();

        var id = order.Id;
        var httpResponse = await _client.PatchAsync($"/orders/{id}", null);
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<OrderView>();

        responseBody?.canceledAt.Should().NotBeNull();
    }

    private Order CreateOrder(String productId)
    {
        var order = new Order(productId);

        return order;
    }

    private void AddOrder(Order order)
    {
        _context.Orders.Add(order);
    }

    private async Task SaveInDb()
    {
        await _context.SaveChangesAsync();
    }
}

namespace OrderingApi.IntegrationTest;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OrderingApi.Data;
using Xunit;
using FluentAssertions;
using OrderingApi.Producers;
using OrderingApi.Consumers;
using OrderingApi.BackgroundServices;
using OrderingApi.Tests.Fakes;
using Ordering.Tests.Fakes;
using Bogus;

class ResponseError
{
    public string? message { get; set; }
}

public enum OrderStatus
{
    NotSent
}

public class OrderProductResponseBody
{
    public string id { get; set; }
    public string status { get; set; }
    public string productId { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? canceledAt { get; set; }
}

[Collection("Sequential")]
public class OrderProductIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private HttpClient _client;
    private readonly ApplicationContext _context;

    public OrderProductIntegrationTest()
    {
        _client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("..");
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<OrderingProducer, FakeOrderingProducer>();
                    services.AddSingleton<StockConsumer, FakeStockConsumer>();
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
    }

    [Fact]
    public async Task WhenProductIdIsNotProvidedThenShouldGetBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/orders", new { });

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(responseBody?.message, "Product id is required");
    }

    [Fact]
    public async Task GivenAddressInformationIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();

        var response = await _client.PostAsJsonAsync("/orders", new { productId = Guid.NewGuid() });

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Address information was not provided. Please provide a valid \"address\" object."
        );
    }

    [Fact]
    public async Task GivenStreetIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var address = new { };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new { productId = Guid.NewGuid(), address = address }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Address information was not provided. Please provide a valid \"street\" field in \"address\" object."
        );
    }

    [Fact]
    public async Task GivenCityIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");

        var address = new { street = faker.Address.StreetName() };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new { productId = Guid.NewGuid(), address = address }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Address information was not provided. Please provide a valid \"city\" field in \"address\" object."
        );
    }

    [Fact]
    public async Task GivenStateIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var address = new { street = street, city = city };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new { productId = Guid.NewGuid(), address = address }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Address information was not provided. Please provide a valid \"state\" field in \"address\" object."
        );
    }

    [Fact]
    public async Task GivenZipCodeIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var address = new
        {
            street = street,
            city = city,
            state = state
        };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new { productId = Guid.NewGuid(), address = address }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Address information was not provided. Please provide a valid \"zipCode\" field in \"address\" object."
        );
    }

    [Fact]
    public async Task GivenPaymentInformationIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = faker.Address.ZipCode()
        };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new { productId = Guid.NewGuid(), address = address }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Payment information was not provided. Please provide a valid \"payment\" object."
        );
    }

    [Fact]
    public async Task GivenPaymentMethodIsNotProvidedWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = faker.Address.ZipCode()
        };
        var payment = new { };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = Guid.NewGuid(),
                address = address,
                payment = payment
            }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Payment information was not provided. Please provide a valid \"method\" field in \"payment\" object."
        );
    }

    [Fact]
    public async Task GivenInvalidPaymentMethodWhenOrderProductThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = faker.Address.ZipCode()
        };
        var payment = new { method = "INVALID_METHOD" };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = Guid.NewGuid(),
                address = address,
                payment = payment
            }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            responseBody?.message,
            "Payment information was not provided. Please provide for \"method\" field in \"payment\" object either: \"BOLETO\" or \"CREDIT_CARD\"."
        );
    }

    [Fact]
    public async Task GivenCreditCardAsPaymentMethodWhenOrderProductThenShouldGetCreated()
    {
        var productId = Guid.NewGuid();
        var stock = new Stock();
        stock.productId = productId.ToString();
        stock.id = Guid.NewGuid();
        stock.quantity = 2;
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var zipCode = faker.Address.ZipCode();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = zipCode
        };
        var payment = new { method = "CREDIT_CARD", cardNumber = faker.Finance.CreditCardNumber() };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = productId,
                address = address,
                payment = payment
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GivenCardNumberIsNotProvidedWhenCreditCardPaymentMethodIsChooseThenShouldGetBadRequest()
    {
        var productId = Guid.NewGuid();
        var stock = new Stock();
        stock.productId = productId.ToString();
        stock.id = Guid.NewGuid();
        stock.quantity = 2;
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var zipCode = faker.Address.ZipCode();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = zipCode
        };
        var payment = new { method = "CREDIT_CARD" };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = productId,
                address = address,
                payment = payment
            }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal(
            responseBody?.message,
            "Credit card information was not provided. Please provide a valid \"cardNumber\" field in \"payment\" object."
        );
    }

    [Fact]
    public async Task WhenValidInputIsProvidedThenShouldStoreOrderInDatabase()
    {
        var productId = Guid.NewGuid();
        var stock = new Stock();
        stock.productId = productId.ToString();
        stock.id = Guid.NewGuid();
        stock.quantity = 2;
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var zipCode = faker.Address.ZipCode();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = zipCode
        };
        var payment = new { method = "BOLETO" };
        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = productId,
                address = address,
                payment = payment
            }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<OrderProductResponseBody>();

        var order = await _context.Orders.FindAsync(responseBody?.id);

        order.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenValidInputToOrderProductWhenProductIsOutOfStockThenShouldGetNotFound()
    {
        var productId = Guid.NewGuid();
        var faker = new Faker("en");
        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var zipCode = faker.Address.ZipCode();
        var address = new
        {
            street = street,
            city = city,
            state = state,
            zipCode = zipCode
        };
        var payment = new { method = "BOLETO" };

        var response = await _client.PostAsJsonAsync(
            "/orders",
            new
            {
                productId = productId,
                address = address,
                payment = payment
            }
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseError>();
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(responseBody?.message, "Product is out of stock");
    }
}

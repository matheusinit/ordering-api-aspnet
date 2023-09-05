namespace OrderingApi;

using Xunit;

public class ProductUnitTesting
{
    [Fact]
    public void WhenNameIsNotProvidedShouldThrow()
    {
        var exception = Record.Exception(() => new Product(_name: null, _price: 0));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void WhenPriceIsNotProvidedThenShouldThrow()
    {
        var exception = Record.Exception(
            () => new Product(_name: "Rustic Frozen Pizza", _price: null)
        );

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void WhenPriceProvidedIsLessThanZeroThenShouldThrow()
    {
        var exception = Record.Exception(
            () => new Product(_name: "Rustic Frozen Pizza", _price: -1)
        );

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void WhenDescriptionIsNotProvidedThenShouldCreateSuccessfully()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _description: null);

        Assert.NotNull(product);
    }

    [Fact]
    public void WhenCreatedAtIsNotProvidedThenShouldCreateSuccessfully()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _createdAt: null);

        Assert.NotNull(product);
    }

    [Fact]
    public void WhenCreatedAtIsProvidedThenShouldCreateSuccessfullyWithData()
    {
        var createdAt = new DateTime();
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _createdAt: createdAt);

        Assert.Equal(product.CreatedAt, createdAt);
    }

    [Fact]
    public void WhenUpdatedAtIsNotProvidedThenShouldCreateSuccessfully()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _updatedAt: null);

        Assert.NotNull(product);
    }

    [Fact]
    public void WhenUpdatedAtIsProvidedThenShouldCreateSuccessfullyWithData()
    {
        var currentDateTime = DateTime.Now;
        var product = new Product(
            _name: "Rustic Frozen Pizza",
            _price: 0,
            _updatedAt: currentDateTime
        );

        Assert.Equal(product.UpdatedAt, currentDateTime);
    }

    [Fact]
    public void WhenDeletedAtIsNotProvidedThenShouldCreateSuccessfully()
    {
        var product = new Product(_name: "Rustic Frozen Pizza", _price: 0, _deletedAt: null);

        Assert.NotNull(product);
    }

    [Fact]
    public void WhenDeletedAtIsProvidedThenShouldCreateSuccessfullyWithData()
    {
        var currentDateTime = DateTime.Now;
        var product = new Product(
            _name: "Rustic Frozen Pizza",
            _price: 0,
            _deletedAt: currentDateTime
        );

        Assert.Equal(product.DeletedAt, currentDateTime);
    }
}

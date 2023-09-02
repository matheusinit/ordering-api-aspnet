namespace OrderingApi;

using Xunit;

/*
 * TODO
 *
 * Properties:
 *  - Name
 *  - Price
 *  - Description
 *  - CreatedAt
 *  - UpdatedAt
 *  - DeletedAt
 */

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
}

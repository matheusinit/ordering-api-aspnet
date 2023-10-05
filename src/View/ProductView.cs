namespace OrderingApi.View;

using OrderingApi.Domain;

public class ProductView
{
    public string id { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
    public string? description { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? deletedAt { get; set; }

    public void setValues(Product product)
    {
        id = product.Id;
        name = product.Name;
        var priceInDecimal = Decimal.Divide((decimal)product?.Price, 100.0m);

        var priceFormatted = decimal.Round(priceInDecimal, 2);

        price = priceFormatted;
        description = product.Description;
        createdAt = product.CreatedAt;
        updatedAt = product.UpdatedAt;
        deletedAt = product.DeletedAt;
    }
}

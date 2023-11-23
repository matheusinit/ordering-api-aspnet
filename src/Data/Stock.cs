namespace OrderingApi.Data;

public class Stock
{
    public Guid id { get; set; }
    public string productId { get; set; }
    public int quantity { get; set; }
}

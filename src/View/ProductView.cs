namespace OrderingApi.View;

public class ProductView
{
    public required string id { get; set; }
    public required string name { get; set; }
    public decimal price { get; set; }
    public string? description { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? deletedAt { get; set; }
}

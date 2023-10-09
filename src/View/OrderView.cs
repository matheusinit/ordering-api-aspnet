public class OrderView
{
    public string id { get; set; }
    public string status { get; set; }
    public string productId { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? cancelAt { get; set; }
}

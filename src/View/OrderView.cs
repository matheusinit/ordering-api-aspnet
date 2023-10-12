using OrderingApi.Data;

public class OrderView
{
    public string id { get; set; }
    public string status { get; set; }
    public string productId { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? canceledAt { get; set; }

    public void setValues(
        string _id,
        OrderStatus _status,
        string _productId,
        DateTime _createdAt,
        DateTime? _updatedAt,
        DateTime? _canceledAt
    )
    {
        this.id = _id;
        this.status = setStatusOnEnum(_status);
        this.productId = _productId;
        this.createdAt = _createdAt;
        this.updatedAt = _updatedAt;
        this.canceledAt = _canceledAt;
    }

    private string setStatusOnEnum(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.NotSent => "Not sent",
            _ => throw new Exception("Invalid status order"),
        };
    }
}

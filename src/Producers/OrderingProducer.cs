using OrderingApi.Data;

namespace OrderingApi.Producers;

public class OrderToProduce
{
    public string id { get; set; }
    public string productId { get; set; }
    public OrderStatus status { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public DateTime? canceledAt { get; set; }
}

public interface OrderingProducer
{
    public Task<bool> SendOrderThroughMessageQueue(string topic, OrderToProduce order);
}

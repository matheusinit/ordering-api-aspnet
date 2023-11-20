namespace OrderingApi.Domain;

using OrderingApi.Data;
using NodaTime;
using System;

public class Order
{
    private string id;

    public Order(String productId)
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Status = OrderStatus.NotSent;
        if (productId == null)
        {
            throw new ArgumentException("ProductId cannot be null");
        }
        ProductId = productId.ToString();
    }

    public string Id
    {
        get => id;
        private set { id = value; }
    }

    public string ProductId { get; set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }

    public void Cancel()
    {
        LocalDateTime localCreatedAt = LocalDateTime.FromDateTime(CreatedAt);
        var aDayAfterOrderWasMade = localCreatedAt.PlusHours(24);

        if (aDayAfterOrderWasMade < LocalDateTime.FromDateTime(DateTime.Now))
        {
            throw new Exception("Order cannot be canceled. Order was made more than 24 hours ago");
        }

        Status = OrderStatus.Canceled;
        CanceledAt = DateTime.Now;
    }
}

namespace OrderingApi.Domain;

using OrderingApi.Data;

public class Order
{
    private string id;
    private Product product;

    public Order(Product _product)
    {
        Product = _product;
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Status = OrderStatus.NotSent;
    }

    private Order()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Status = OrderStatus.NotSent;
    }

    public string Id
    {
        get => id;
        private set { id = value; }
    }

    public string ProductId { get; set; }

    public Product Product
    {
        get => product;
        private set
        {
            if (value == null)
            {
                throw new ArgumentException("Product cannot be null");
            }

            product = value;
        }
    }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }

    public void Cancel()
    {
        CanceledAt = DateTime.Now;
    }
}

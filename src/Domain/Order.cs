namespace OrderingApi.Domain;

public class Order
{
    private Product product;
    private string id;

    public Order(Product _product)
    {
        Product = product = _product;
        Id = Guid.NewGuid().ToString();
    }

    public string Id
    {
        get => id;
        set { id = value; }
    }

    public Product Product
    {
        get => product;
        private set
        {
            if (value == null)
            {
                throw new ArgumentException("Product cannot be null");
            }
        }
    }
}
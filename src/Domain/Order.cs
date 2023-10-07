namespace OrderingApi.Domain;

public class Order
{
    private string id;
    private Product product;

    public Order(Product _product)
    {
        Product = _product;
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

            product = value;
        }
    }
}

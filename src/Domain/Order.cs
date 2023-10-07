namespace OrderingApi.Domain;

public class Order
{
    private Product product;

    public Order(Product _product)
    {
        Product = product = _product;
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

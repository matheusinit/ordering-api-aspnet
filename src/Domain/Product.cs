namespace OrderingApi;

public class Product
{
    public Product(string _name, int? _price)
    {
        Name = _name;
        Price = _price;
    }

    public string Name
    {
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty");
            }
        }
    }

    public int? Price
    {
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Price cannot be empty");
            }
        }
    }
}

namespace OrderingApi;

public class Product
{
    public Product(string _name, int? _price, string? _description = null)
    {
        Name = _name;
        Price = _price;
        Description = _description;
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

    public string Description { get; set; }
}

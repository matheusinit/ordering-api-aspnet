namespace OrderingApi;

public class Product
{
    public Product(string _name)
    {
        Name = _name;
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
}

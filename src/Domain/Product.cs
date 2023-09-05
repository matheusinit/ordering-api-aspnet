namespace OrderingApi;

public class Product
{
    public Product(
        string _name,
        int? _price,
        string? _description = null,
        DateTime? _createdAt = null,
        DateTime? _updatedAt = null,
        DateTime? _deletedAt = null
    )
    {
        Name = _name;
        Price = _price;
        Description = _description;

        if (_createdAt == null)
        {
            CreatedAt = DateTime.Now;
        }
        else
        {
            CreatedAt = _createdAt;
        }

        UpdatedAt = _updatedAt;
        DeletedAt = _deletedAt;
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
            else if (value < 0)
            {
                throw new ArgumentException("Price cannot be less than zero");
            }
        }
    }

    public string? Description { get; set; }
    public DateTime? CreatedAt { get; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

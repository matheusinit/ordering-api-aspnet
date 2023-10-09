namespace OrderingApi.Domain;

public class Product
{
    private string name;
    private int price;
    private string? description;

    public Product() { }

    public Product(
        string? _name,
        int? _price,
        string? _description = null,
        DateTime? _createdAt = null,
        DateTime? _updatedAt = null,
        DateTime? _deletedAt = null
    )
    {
        Id = Guid.NewGuid().ToString();
        Name = _name;
        Price = _price;
        Description = _description;

        if (_createdAt == null)
        {
            CreatedAt = DateTime.Now;
        }
        else
        {
            CreatedAt = (DateTime)_createdAt;
        }

        UpdatedAt = _updatedAt;
        DeletedAt = _deletedAt;
    }

    public string? Id { get; set; }

    public string Name
    {
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty");
            }

            name = value;
        }
        get => this.name;
    }

    public int? Price
    {
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Price cannot be empty");
            }

            if (value < 0)
            {
                throw new ArgumentException("Price cannot be less than zero");
            }

            price = (int)value;
        }
        get => this.price;
    }

    public string? Description
    {
        set
        {
            if (value == "")
            {
                throw new ArgumentException("Description cannot be a empty string");
            }

            description = value;
        }
        get => this.description;
    }

    public List<Order> Orders { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

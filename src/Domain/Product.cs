namespace OrderingApi.Domain;

public class Product
{
    private string name;
    private int? price;
    private string? description;

    public Product() { }

    public Product(
        string _name,
        int? _price,
        string? _description = null,
        DateTime? _createdAt = null,
        DateTime? _updatedAt = null,
        DateTime? _deletedAt = null,
        string? _id = null
    )
    {
        Id = _id;
        Name = name = _name;
        Price = price = _price;
        Description = description = _description;

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

    public string? Id { get; set; }

    public virtual string Name
    {
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty");
            }
        }
        get { return this.name; }
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
        get { return this.price; }
    }

    public string? Description
    {
        set
        {
            if (value == "")
            {
                throw new ArgumentException("Description cannot be a empty string");
            }
        }
        get { return this.description; }
    }
    public DateTime? CreatedAt { get; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

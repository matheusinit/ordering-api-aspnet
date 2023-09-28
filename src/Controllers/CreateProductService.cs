namespace OrderingApi.Controllers;

using OrderingApi.Data;

public class ProductServiceRequest
{
    public string? name { get; set; }
    public int? price { get; set; }
    public string? description { get; set; }
}

public class CreateProductService
{
    private readonly ApplicationContext _context;

    public CreateProductService(ApplicationContext context)
    {
        _context = context;
    }

    public virtual Domain.Product createProduct(ProductServiceRequest product)
    {
        var productEntity = new Domain.Product(
            _name: product?.name,
            _price: product?.price,
            _description: product?.description,
            _id: Guid.NewGuid().ToString()
        );

        _context.Products.Add(productEntity);
        _context.SaveChanges();

        return productEntity;
    }
}

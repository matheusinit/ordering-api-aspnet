namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

public class Product
{
    public string? id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
    public string? description { get; set; }
    public DateTime createdAt { get; set; }
}

[ApiController]
[Route("/products")]
public class CreateProductController : ControllerBase
{
    private readonly ILogger<CreateProductController> _logger;

    public CreateProductController(ILogger<CreateProductController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<HttpResponse> Create([FromBody] Product product)
    {
        if (product.name == "")
        {
            return BadRequest(error: new { message = "Name is required" });
        }

        if (product.price == 0)
        {
            return BadRequest(error: new { message = "Price is required" });
        }

        if (product.price < 0)
        {
            return BadRequest(error: new { message = "Price cannot be less than zero" });
        }

        var dbContext = new ApplicationContext();

        var productEntity = new Domain.Product(
            _name: product.name,
            _price: product.price,
            _description: product.description,
            _id: Guid.NewGuid().ToString()
        );

        dbContext.Products.Add(productEntity);
        dbContext.SaveChanges();

        return Created(
            "",
            new
            {
                id = productEntity.Id,
                name = product.name,
                price = product.price,
                description = product.description,
                createdAt = product.createdAt
            }
        );
    }
}

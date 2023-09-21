namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

public class Product
{
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
        if (product.price == 0)
        {
            return BadRequest(error: new { message = "Price is required" });
        }

        if (product.price < 0)
        {
            return BadRequest(error: new { message = "Price cannot be less than zero" });
        }

        return Created(
            "",
            new Product
            {
                name = product.name,
                price = product.price,
                description = product.description,
                createdAt = DateTime.Now
            }
        );
    }
}

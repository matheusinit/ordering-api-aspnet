namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

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
    private readonly CreateProductService _service;

    public CreateProductController(
        ILogger<CreateProductController> logger,
        CreateProductService service
    )
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public ActionResult<HttpResponse> Create([FromBody] Product product)
    {
        try
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

            var productEntity = _service.createProduct(
                new ProductInput
                {
                    name = product.name,
                    price = product.price,
                    description = product.description
                }
            );

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
        catch (Exception)
        {
            return StatusCode(500, new { message = "An internal server error occured" });
        }
    }
}

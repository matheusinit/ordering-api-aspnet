namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

public class ProductRequestBody
{
    public string? id { get; set; }
    public string? name { get; set; }
    public int? price { get; set; }
    public string? description { get; set; }
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
    public ActionResult<HttpResponse> Create([FromBody] ProductRequestBody product)
    {
        try
        {
            var input = new ProductServiceRequest
            {
                name = product.name,
                price = product.price,
                description = product.description
            };

            var productEntity = _service.createProduct(input);

            var uri = new Uri(
                $"{Request.Scheme}://{Request.Host}{Request.PathBase}/products/{productEntity.Id}"
            );

            return Created(uri, productEntity);
        }
        catch (Exception error)
        {
            if (error.Message == "Name cannot be empty")
            {
                return BadRequest(error: new { message = "Name is required" });
            }

            if (error.Message == "Price cannot be empty")
            {
                return BadRequest(error: new { message = "Price is required" });
            }

            if (error.Message == "Price cannot be less than zero")
            {
                return BadRequest(error: new { message = "Price cannot be less than zero" });
            }

            return StatusCode(500, new { message = "An internal server error occured" });
        }
    }
}

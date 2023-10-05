namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;

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
    private readonly ApplicationContext _context;

    public CreateProductController(
        ILogger<CreateProductController> logger,
        ApplicationContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public ActionResult<HttpResponse> Create([FromBody] ProductRequestBody product)
    {
        try
        {
            var productEntity = new Product(
                _name: product?.name,
                _price: product?.price,
                _description: product?.description,
                _id: Guid.NewGuid().ToString()
            );

            _context.Products.Add(productEntity);
            _context.SaveChanges();

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

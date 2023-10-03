namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderingApi.Data;

class UpdateRequestBody
{
    public string? name { get; set; }
    public int? price { get; set; }
    public string? description { get; set; }
}

[ApiController]
[Route("/products")]
public class UpdateProductController : ControllerBase
{
    private readonly ILogger<UpdateProductController> _logger;
    private readonly ApplicationContext _context;

    public UpdateProductController(
        ILogger<UpdateProductController> logger,
        ApplicationContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    [HttpPut("{id}")]
    public ActionResult<HttpResponse> Update(string id, [FromBody] ProductRequestBody product)
    {
        var productFound = _context.Products.Find(id);

        if (productFound == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        if (!product.name.IsNullOrEmpty())
        {
            productFound.Name = product.name;
        }

        if (product.price != null)
        {
            productFound.Price = product.price;
        }

        if (product.description != null)
        {
            productFound.Description = product.description;
        }

        productFound.UpdatedAt = DateTime.Now;
        _context.SaveChanges();

        var productView = new
        {
            Id = productFound.Id,
            Name = product.name ?? productFound.Name,
            Price = (product.price / 100.0) ?? productFound.Price / 100.0,
            Description = product.description ?? productFound.Description,
            CreatedAt = productFound.CreatedAt,
            UpdatedAt = productFound.UpdatedAt,
            DeletedAt = productFound.DeletedAt
        };

        return Ok(productView);
    }
}

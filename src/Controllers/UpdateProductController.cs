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

        if (product.name != null)
        {
            productFound.Name = product.name;
        }
        _context.SaveChanges();

        return Ok(productFound);
    }
}

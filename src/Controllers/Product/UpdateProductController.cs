namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderingApi.Data;
using OrderingApi.View;

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
        if (product.name == "")
        {
            return BadRequest(new { message = "Name is required" });
        }

        if (product.description == "")
        {
            return BadRequest(new { message = "Description is required" });
        }

        var productFound = _context.Products.Find(id);

        if (productFound == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        if (!product.name.IsNullOrEmpty())
        {
            _context.Entry(productFound).Property("Name").CurrentValue = product.name;
        }

        if (product.price != null)
        {
            _context.Entry(productFound).Property("Price").CurrentValue = product.price;
        }

        if (product.description != null)
        {
            _context.Entry(productFound).Property("Description").CurrentValue = product.description;
        }

        productFound.UpdatedAt = DateTime.Now;
        _context.SaveChanges();

        var view = new ProductView();
        view.setValues(productFound);

        return Ok(view);
    }
}

namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;

// Usecase
// 1. Check if product id is not nul
// 2. Get product by id
// 3. Create order

public class OrderProductRequest
{
    public string? productId { get; set; }
}

[ApiController]
[Route("/order")]
public class OrderProductController : ControllerBase
{
    private readonly ApplicationContext _context;

    public OrderProductController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpPost]
    public ActionResult<HttpResponse> Order([FromBody] OrderProductRequest request)
    {
        var product = _context.Products.Find(request.productId);

        if (request.productId == null)
        {
            return BadRequest(error: new { message = "Product id is required" });
        }

        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        var order = new Order(_product: product);

        var view = new
        {
            id = order.Id,
            status = "Not sent",
            productId = order.Product.Id,
            createdAt = order.CreatedAt,
            updatedAt = order.UpdatedAt,
            cancelAt = order.CanceledAt
        };

        return Ok(view);
    }
}

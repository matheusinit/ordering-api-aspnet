namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

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

    public ActionResult<HttpResponse> Order([FromBody] OrderProductRequest order)
    {
        var product = _context.Products.Find(order.productId);

        if (order.productId == null)
        {
            return BadRequest(error: new { message = "Product id is required" });
        }

        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        return Ok(new { productId = product.Id });
    }
}

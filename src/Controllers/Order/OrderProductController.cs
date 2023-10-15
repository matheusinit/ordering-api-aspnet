namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;

public class OrderProductRequest
{
    public string? productId { get; set; }
}

[ApiController]
[Route("/orders")]
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
        try
        {
            var product = _context.Products.Find(request.productId);

            if (request.productId == null)
            {
                return BadRequest(error: new { message = "Product id is required" });
            }

            var order = new Order(_product: product);

            _context.Orders.Add(order);
            _context.SaveChanges();

            var view = new OrderView();

            view.setValues(
                order.Id,
                order.Status,
                order.ProductId,
                order.CreatedAt,
                order.UpdatedAt,
                order.CanceledAt
            );

            return Ok(view);
        }
        catch (Exception error)
        {
            if (error.Message == "Product cannot be null")
            {
                return NotFound(new { message = "Product not found" });
            }

            return StatusCode(500, new { message = "An inner error occurred. Try again later." });
        }
    }
}

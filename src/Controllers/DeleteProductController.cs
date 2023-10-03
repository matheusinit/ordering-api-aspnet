namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

[ApiController]
[Route("/products")]
public class DeleteProductController : ControllerBase
{
    private readonly ILogger<DeleteProductController> _logger;
    private readonly ApplicationContext _context;

    public DeleteProductController(
        ILogger<DeleteProductController> logger,
        ApplicationContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    [HttpDelete("{id}")]
    public ActionResult<HttpResponse> Delete(string id)
    {
        var productFound = _context.Products.Find(id);

        if (productFound == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        return NoContent();
    }
}

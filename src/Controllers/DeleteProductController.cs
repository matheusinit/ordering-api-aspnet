namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/products")]
public class DeleteProductController : ControllerBase
{
    private readonly ILogger<DeleteProductController> _logger;

    public DeleteProductController(ILogger<DeleteProductController> logger)
    {
        _logger = logger;
    }

    [HttpDelete("{id}")]
    public ActionResult<HttpResponse> Delete(string id)
    {
        return NotFound(new { message = "Product not found" });
    }
}

namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

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

    public UpdateProductController(ILogger<UpdateProductController> logger)
    {
        _logger = logger;
    }

    [HttpPut("{id}")]
    public ActionResult<HttpResponse> Update(string id, [FromBody] ProductRequestBody product)
    {
        return NotFound(new { message = "Product not found" });
    }
}

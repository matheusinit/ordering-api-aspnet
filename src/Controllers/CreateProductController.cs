namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

public class Product
{
    public string name { get; set; }
    public int price { get; set; }
}

[ApiController]
[Route("/products")]
public class CreateProductController : ControllerBase
{
    private readonly ILogger<CreateProductController> _logger;

    public CreateProductController(ILogger<CreateProductController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<HttpResponse> Create([FromBody] Product product)
    {
        if (product.price == 0)
        {
            return BadRequest(error: new { message = "Price is required" });
        }

        return BadRequest(error: new { message = "" });
    }
}

namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/products")]
public class CreateProductController : ControllerBase
{
    private readonly ILogger<CreateProductController> _logger;

    public CreateProductController(ILogger<CreateProductController> logger)
    {
        _logger = logger;
    }

    public ActionResult Get()
    {
        return BadRequest();
    }
}

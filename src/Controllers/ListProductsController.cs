namespace OrderingApi.Controllers;

using System.Collections;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/products")]
public class ListProductsController : ControllerBase
{
    private readonly ILogger<ListProductsController> _logger;

    public ListProductsController(ILogger<ListProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<HttpResponse> List()
    {
        return Ok(new ArrayList());
    }
}

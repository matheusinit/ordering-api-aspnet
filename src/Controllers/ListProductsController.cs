namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;

[ApiController]
[Route("/products")]
public class ListProductsController : ControllerBase
{
    private readonly ILogger<ListProductsController> _logger;
    private readonly ApplicationContext _context;

    public ListProductsController(
        ILogger<ListProductsController> logger,
        ApplicationContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public ActionResult<HttpResponse> List()
    {
        var products = _context.Products.ToList<Product>();

        return Ok(products);
    }
}

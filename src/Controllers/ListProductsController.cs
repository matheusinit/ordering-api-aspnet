namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;
using OrderingApi.View;

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
        var products = _context.Products
            .ToList<Product>()
            .Select(p =>
            {
                var view = new ProductView();
                view.setValues(p);
                return view;
            })
            .ToList();

        return Ok(products);
    }
}

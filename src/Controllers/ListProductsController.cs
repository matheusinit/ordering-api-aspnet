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
        var products = _context.Products
            .ToList<Product>()
            .Select(p =>
            {
                var priceInDecimal = Decimal.Divide((decimal)p?.Price, 100.0m);

                var priceFormatted = decimal.Round(priceInDecimal, 2);

                return new
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = priceFormatted,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    DeletedAt = p.DeletedAt,
                };
            })
            .ToList();

        return Ok(products);
    }
}

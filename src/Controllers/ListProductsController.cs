namespace OrderingApi.Controllers;

using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
                var priceInDouble = (p.Price / 100.00) ?? 0.00;
                var priceFormatted = double.Round(priceInDouble, 2);

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

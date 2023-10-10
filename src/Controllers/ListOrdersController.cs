using System.Collections;
using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

namespace OrderingApi.Controllers;

[ApiController]
[Route("/orders")]
public class ListOrdersController : ControllerBase
{
    private readonly ApplicationContext _context;

    public ListOrdersController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<HttpResponse> List()
    {
        var orders = _context.Orders.ToList();

        var view = orders.Select(o =>
        {
            return new OrderView
            {
                id = o.Id,
                status = "Not sent",
                productId = o.ProductId,
                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt,
                cancelAt = o.CanceledAt
            };
        });

        return Ok(view);
    }
}

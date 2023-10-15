namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

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
            var view = new OrderView();

            view.setValues(
                _id: o.Id,
                _status: o.Status,
                _productId: o.ProductId,
                _createdAt: o.CreatedAt,
                _updatedAt: o.UpdatedAt,
                _canceledAt: o.CanceledAt
            );

            return view;
        });

        return Ok(view);
    }
}

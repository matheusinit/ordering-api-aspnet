namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;

[ApiController]
[Route("/orders")]
public class CancelOrderController : ControllerBase
{
    private readonly ApplicationContext context;

    public CancelOrderController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpPatch("{id}")]
    public ActionResult<HttpResponse> Cancel(string id)
    {
        var result = Guid.Empty;
        var isIdAGuid = Guid.TryParse(id, out result);
        if (!isIdAGuid)
        {
            return BadRequest();
        }

        var order = context.Orders.Find(id);

        if (order == null)
        {
            return NotFound();
        }

        if (order.CanceledAt != null)
        {
            return BadRequest(error: new { message = "Order is already canceled" });
        }

        context.Entry(order).Property("CanceledAt").CurrentValue = DateTime.Now;
        context.SaveChanges();

        var view = new OrderView();

        view.setValues(
            _id: order.Id,
            _status: order.Status,
            _productId: order.ProductId,
            _createdAt: order.CreatedAt,
            _updatedAt: order.UpdatedAt,
            _canceledAt: order.CanceledAt
        );

        return Ok(view);
    }
}

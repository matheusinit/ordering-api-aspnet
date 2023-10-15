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
        try
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

            order.Cancel();

            context.Entry(order).Property("CanceledAt").CurrentValue = order.CanceledAt;
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
        catch (Exception exception)
        {
            if (
                exception.Message
                == "Order cannot be canceled. Order was made more than 24 hours ago"
            )
            {
                return BadRequest(error: new { message = exception.Message });
            }

            return BadRequest();
        }
    }
}

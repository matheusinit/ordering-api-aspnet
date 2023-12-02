namespace OrderingApi.Controllers;

using System;
using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;
using OrderingApi.Producers;

public class OrderProductRequest
{
    public string? productId { get; set; }
}

[ApiController]
[Route("/orders")]
public class OrderProductController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly OrderingProducer _producer;

    public OrderProductController(ApplicationContext context, OrderingProducer producer)
    {
        _context = context;
        _producer = producer;
    }

    [HttpPost]
    public async Task<ActionResult<HttpResponse>> Order([FromBody] OrderProductRequest request)
    {
        try
        {
            if (request.productId == null)
            {
                return BadRequest(error: new { message = "Product id is required" });
            }

            var stock = _context.Stocks
                .Where(s => s.productId == request.productId)
                .FirstOrDefault();

            if (stock == null)
            {
                return NotFound(new { message = "Product is out of stock" });
            }

            var order = new Order(request.productId);

            _context.Orders.Add(order);
            _context.SaveChanges();

            var view = new OrderView();

            view.setValues(
                order.Id,
                order.Status,
                order.ProductId,
                order.CreatedAt,
                order.UpdatedAt,
                order.CanceledAt
            );

            var orderToSendThroughMessageQueue = new OrderToProduce
            {
                id = order.Id,
                productId = order.ProductId,
                status = order.Status,
                createdAt = order.CreatedAt,
                updatedAt = order.UpdatedAt,
                canceledAt = order.CanceledAt
            };

            await _producer.SendOrderThroughMessageQueue(
                "ordering",
                orderToSendThroughMessageQueue
            );

            return Ok(view);
        }
        catch (Exception error)
        {
            if (error.Message == "Product cannot be null")
            {
                return NotFound(new { message = "Product not found" });
            }

            return StatusCode(500, new { message = "An inner error occurred. Try again later." });
        }
    }
}

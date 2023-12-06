namespace OrderingApi.Controllers;

using System;
using Microsoft.AspNetCore.Mvc;
using OrderingApi.Data;
using OrderingApi.Domain;
using OrderingApi.Producers;

public class AddressInfo
{
    public string? street { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? zipCode { get; set; }
}

public class PaymentInfo
{
    public string? method { get; set; }
    public string? cardNumber { get; set; }
    public int? expirationMonth { get; set; }
    public int? expirationYear { get; set; }
    public int? cvc { get; set; }
}

public class OrderProductRequest
{
    public string? productId { get; set; }
    public AddressInfo? address { get; set; }
    public PaymentInfo? payment { get; set; }
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

            if (request.address == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Address information was not provided. Please provide a valid \"address\" object."
                    }
                );
            }

            if (request.address.street == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Address information was not provided. Please provide a valid \"street\" field in \"address\" object."
                    }
                );
            }

            if (request.address.city == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Address information was not provided. Please provide a valid \"city\" field in \"address\" object."
                    }
                );
            }

            if (request.address.state == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Address information was not provided. Please provide a valid \"state\" field in \"address\" object."
                    }
                );
            }

            if (request.address.zipCode == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Address information was not provided. Please provide a valid \"zipCode\" field in \"address\" object."
                    }
                );
            }

            if (request.payment == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Payment information was not provided. Please provide a valid \"payment\" object."
                    }
                );
            }

            if (request.payment.method == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Payment information was not provided. Please provide a valid \"method\" field in \"payment\" object."
                    }
                );
            }

            var payment = new Payment(
                request.payment.method,
                request.payment.cardNumber,
                request.payment.expirationMonth,
                request.payment.expirationYear,
                request.payment.cvc
            );

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

            if (error.Message == "Invalid payment method")
            {
                return BadRequest(
                    new
                    {
                        message = "Payment information was not provided. Please provide for \"method\" field in \"payment\" object either: \"BOLETO\" or \"CREDIT_CARD\"."
                    }
                );
            }

            if (
                error.Message
                == "Invalid payment method. Credit card method is required to set card number"
            )
            {
                return BadRequest(
                    new
                    {
                        message = "Credit card information was not provided. Please provide a valid \"cardNumber\" field in \"payment\" object."
                    }
                );
            }

            if (
                error.Message
                == "Invalid payment method. Credit card method is required to set expiration month"
            )
            {
                return BadRequest(
                    new
                    {
                        message = "Credit card information was not provided. Please provide a valid \"expirationMonth\" field from 0-12 in \"payment\" object."
                    }
                );
            }

            if (
                error.Message
                == "Invalid payment method. Credit card method is required to set expiration year"
            )
            {
                return BadRequest(
                    new
                    {
                        message = "Credit card information was not provided. Please provide a valid \"expirationYear\" field in format YYYY in \"payment\" object."
                    }
                );
            }

            if (
                error.Message == "Invalid payment method. Credit card method is required to set cvc"
            )
            {
                return BadRequest(
                    new
                    {
                        message = "Credit card information was not provided. Please provide a valid \"cvc\" field in \"payment\" object."
                    }
                );
            }

            return StatusCode(500, new { message = "An inner error occurred. Try again later." });
        }
    }
}

namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/order")]
public class OrderProductController : ControllerBase
{
    public ActionResult<HttpResponse> Order()
    {
        return BadRequest(error: new { message = "Product id is required" });
    }
}

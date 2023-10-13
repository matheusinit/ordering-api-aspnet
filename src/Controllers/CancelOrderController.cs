namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/orders")]
public class CancelOrderController : ControllerBase
{
    [HttpPatch("{id}")]
    public ActionResult<HttpResponse> Cancel()
    {
        return BadRequest();
    }
}

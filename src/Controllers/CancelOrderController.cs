namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/orders")]
public class CancelOrderController : ControllerBase
{
    [HttpPatch("{id}")]
    public ActionResult<HttpResponse> Cancel(string id)
    {
        var result = Guid.Empty;
        var isIdAGuid = Guid.TryParse(id, out result);
        if (!isIdAGuid)
        {
            return BadRequest();
        }

        return NotFound();
    }
}

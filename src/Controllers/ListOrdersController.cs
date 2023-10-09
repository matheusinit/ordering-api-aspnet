using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace OrderingApi.Controllers;

[ApiController]
[Route("/orders")]
public class ListOrdersController : ControllerBase
{
    [HttpGet]
    public ActionResult<HttpResponse> List()
    {
        return Ok(new ArrayList());
    }
}

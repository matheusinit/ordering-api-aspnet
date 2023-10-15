namespace OrderingApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api")]
public class ApiInfoController : ControllerBase
{
    [HttpGet]
    public ActionResult<HttpResponse> GetInstanceInfo()
    {
        var hostname = System.Environment.MachineName;

        return Ok(new { hostname = hostname });
    }
}

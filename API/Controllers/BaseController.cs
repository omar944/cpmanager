using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool UserIsAuthenticated() => HttpContext.User.Identity?.IsAuthenticated ?? false;
}
using API.Extensions;
using API.Interfaces;
using Entities.App;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController : ControllerBase
{
    protected readonly IUserRepository Users;

    protected BaseController(IUserRepository users)
    {
        Users = users;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    
    protected async Task<User> GetUser(bool withCodeforces=false)
    {
        var user = await Users.GetUserByIdAsync(User.GetUserId(),withCodeforces);
        return user!;
    }
}
using API.Extensions;
using API.Interfaces;
using Entities.App;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public abstract class BaseController : ControllerBase
{
    protected readonly IUserRepository Users;

    protected BaseController(IUserRepository users)
    {
        Users = users;
    }

    protected async Task<User> GetUser()
    {
        var user = await Users.GetUserByIdAsync(User.GetUserId());
        return user;
    }
}
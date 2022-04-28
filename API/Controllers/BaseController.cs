using API.Extensions;
using API.Interfaces;
using Entities.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController : ControllerBase
{
    protected IUserRepository Users = null!;

    protected async Task<User> GetUser()
    {
        var user = await Users.GetUserByIdAsync(User.GetUserId());
        return user;
    }
}
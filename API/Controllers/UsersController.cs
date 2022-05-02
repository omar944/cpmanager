using API.Data;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository,IMapper mapper):base(repository)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var res = await Users.GetUsersProfilesAsync();
        return Ok(res);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetUser(string username)
    {
        var user = await Users.GetUserProfileAsync(username);
        if (user is null) return NotFound();
        return user;
    }
    
    
}
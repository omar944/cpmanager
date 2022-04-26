using API.Data;
using API.DTOs;
using API.Interfaces;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var res = await _repository.GetUsersProfilesAsync();
        return res;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetUser(string username)
    {
        var user = await _repository.GetUserProfileAsync(username);
        if (user is null) return NotFound();
        return user;
    }
    
    
}
using API.Interfaces;
using API.Models;
using CodeforcesTool.Services;
using Entities.Codeforces;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly CodeforcesApiService _codeforcesService;

    public UsersController(IUserRepository repository, IMapper mapper, CodeforcesApiService codeforcesService) :
        base(repository)
    {
        _mapper = mapper;
        _codeforcesService = codeforcesService;
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

    [HttpPost("codeforces-account")]
    public async Task<IActionResult> AddCodeforcesAccount([FromBody]string handle)
    {
        var account = await _codeforcesService.GetUserAsync(handle);
        if(account is null)return NotFound(new{message="no such handle"});
        
        var user = await GetUser();
        user.CodeforcesAccount = _mapper.Map<CodeforcesAccount>(account);
        
        if (await Users.SaveChangesAsync() == false) return BadRequest();
        
        return Ok();
    }
}
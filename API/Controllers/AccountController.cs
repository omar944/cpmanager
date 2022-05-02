using Entities.App;
using API.Interfaces;
using API.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController:ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResultDto>> Register([FromBody]RegisterDto dto)
    {
        if (await CheckUser(dto.UserName)) return BadRequest(new {error = "existing username"});

        var user = _mapper.Map<User>(dto);
        user.UserName = dto.UserName?.ToLower();
        var res = await _userManager.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        var role = await _userManager.AddToRoleAsync(user, "Member");
        if (!role.Succeeded) return BadRequest(role.Errors);
        
        return new LoginResultDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody]LoginDto dto)
    {
        if (dto.Username is null)
        {
            return BadRequest();
        }
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == dto.Username.ToLower());
        
        if (user == null) return Unauthorized("invalid username");
        var res = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!res.Succeeded) return Unauthorized();
        
        return new LoginResultDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }

    private async Task<bool> CheckUser(string? username)
    {
        if (username is null) return false;
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
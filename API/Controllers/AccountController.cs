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
        if (await CheckEmail(dto.Email)) return BadRequest(new {error = "existing email"});
        dto.Email = dto.Email?.ToLower();
        var user = _mapper.Map<User>(dto);
        user.UserName = dto.Email;
        var res = await _userManager.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        var roles = new List<string> {"Member"};
        if (dto.Status == "coach")
            roles.Add("Coach");
        
        var role = await _userManager.AddToRolesAsync(user, roles);
        if (!role.Succeeded) return BadRequest(role.Errors);

        return new LoginResultDto
        {
            Email = user.Email,
            Token = await _tokenService.CreateToken(user),
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody]LoginDto dto)
    {
        if (dto.Email is null)
        {
            return BadRequest();
        }
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.Email == dto.Email.ToLower());
        
        if (user == null) return Unauthorized("invalid username");
        var res = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (res.Succeeded == false) return Unauthorized();
        
        return new LoginResultDto
        {
            Email = user.Email,
            Token = await _tokenService.CreateToken(user),
        };
    }

    private async Task<bool> CheckEmail(string? email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }
}
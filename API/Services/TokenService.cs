using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.App;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config,UserManager<User> userManager)
    {
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }

    public async Task<string> CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.NameId,user.Id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName,user.Email),
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
        
        var credentials =  new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);

        var tokeDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokeDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
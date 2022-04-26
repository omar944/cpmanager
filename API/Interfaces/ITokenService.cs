using Entities.App;

namespace API.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(User user);
}
using Entities.App;
using Entities.Codeforces;

namespace API.Interfaces;

public interface ICodeforcesService
{
    Task<bool> UpdateSubmissions(User user);
}
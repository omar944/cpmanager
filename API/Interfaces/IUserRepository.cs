using API.Helpers.Pagination;
using API.Models;
using API.Models.Parameters;
using Entities.App;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(User user);
    Task<User?> GetUserByIdAsync(int id, bool withCodeforces = false);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<PagedList<UserDto>> GetUsersProfilesAsync(UserParameters userParams);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<UserDto?> GetUserProfileAsync(int id,bool? owner=false);
    Task<bool> SaveChangesAsync();
    Task<List<User>> GetUsersAsync(List<int>ids);
    public IQueryable<User> GetQuery();
    Task<IEnumerable<UserDto>> GetFilteredUsersProfilesAsync(string q,bool? coachOnly);
}
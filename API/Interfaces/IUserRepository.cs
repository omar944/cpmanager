using API.Models;
using Entities.App;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetUsersProfilesAsync();
    Task<IEnumerable<User>> GetUsersAsync();
    Task<UserDto?> GetUserProfileAsync(int id,bool? owner=false);
    Task<bool> SaveChangesAsync();
    Task<List<User>> GetUsersAsync(List<int>ids);
    public IQueryable<User> GetQuery();
}
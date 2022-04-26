using API.DTOs;
using API.Interfaces;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    private readonly DbSet<User> _users;
    public UserRepository(AppDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _users = context.Set<User>();
    }

    public void Update(User user)
    {
        _users.Update(user);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _users.FindAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _users.SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<List<UserDto>> GetUsersProfilesAsync()
    {
        return await _users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _users.ToListAsync();
    }

    public async Task<UserDto?> GetUserProfileAsync(string username, bool? owner=false)
    {
        return await _users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
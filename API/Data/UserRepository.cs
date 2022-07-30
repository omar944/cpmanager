using API.Helpers.Pagination;
using API.Interfaces;
using API.Models;
using API.Models.Parameters;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    private readonly DbSet<User> _users;
    public UserRepository(AppDbContext context,IMapper mapper,UserManager<User> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
        _users = context.Set<User>();
    }

    public void Update(User user)
    {
        _users.Update(user);
    }

    public async Task<User?> GetUserByIdAsync(int id,bool withCodeforces=false)
    {
        var ret = withCodeforces? await _users.Include(x=>x.CodeforcesAccount)
                .FirstOrDefaultAsync(x=>x.Id==id)
                :await _users.FindAsync(id);
        return ret;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _users.SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<PagedList<UserDto>> GetUsersProfilesAsync(UserParameters userParams)
    {
        var query = _userManager.Users.Include(x => x.UserRoles).ThenInclude(r => r.Role).Include(u => u.Teams)
            .AsNoTracking().AsSplitQuery();

        query = userParams.CoachesOnly?
              query.Where(user => user.UserRoles.Select(x => x.Role.Name).Contains("Coach"))
            : query.Where(user => user.UserRoles.Select(x => x.Role.Name).All(x => x == "Member"));
        
        if (userParams.NotInATeam)
        {
            query = query.Where(user => user.Teams == null || user.Teams.Count == 0);
        }

        var result = query.ProjectTo<UserDto>(_mapper.ConfigurationProvider);

        return await PagedList<UserDto>.CreateAsync(result,
                        userParams.PageNumber, userParams.PageSize);
    }

    public async Task<IEnumerable<UserDto>> GetFilteredUsersProfilesAsync(string q, bool? coachOnly)
    {
        var res = _userManager.Users.Include(x => x.UserRoles)
            .ThenInclude(r => r.Role).AsNoTracking()
            .Where(user => user.FullName!.Contains(q)).AsSplitQuery();
        
        if (coachOnly != null && coachOnly.Value)
        {
            res = res.Where(user=>user.UserRoles.Select(x=>x.Role.Name).Contains("Coach"));
        }
        else
        {
            res = res.Where(user=>user.UserRoles.Select(x=>x.Role.Name).All(x=>x=="Member"));
        }

        return await res.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _users.ToListAsync();
    }

    public async Task<UserDto?> GetUserProfileAsync(int id, bool? owner=false)
    {
        return await _users.Include(x=>x.UserRoles).ThenInclude(r=>r.Role).AsNoTracking()
            .AsSplitQuery()
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<User>> GetUsersAsync(List<int> ids)
    {
        return await _users.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
    
    public IQueryable<User> GetQuery()
    {
        return _users;
    }
}
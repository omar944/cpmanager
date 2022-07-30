using API.Controllers;
using API.Interfaces;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

#pragma warning disable CS8601

namespace API.Data;

public class Seed:BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AppDbContext _context;
    private readonly CodeforcesApiService _apiService;
    private readonly IMapper _mapper;
    
    public Seed(IUserRepository users,UserManager<User> userManager,RoleManager<Role> roleManager
    ,AppDbContext context,CodeforcesApiService apiService,IMapper mapper) 
        : base(users)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _apiService = apiService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult> StartAsync()
    {
        _ = StartSeed();
        return Ok();
    }

    private async Task StartSeed()
    {
        await SeedUsers();
        await SeedCodeforcesUsers();
        await SeedProblems();
        await SeedSubmissions();
    }
    
    private async Task SeedUsers()
    {
        if (await _userManager.Users.AnyAsync()) return;

        var roles = new List<Role>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Coach"},
        };

        foreach (var role in roles)
        {
            await _roleManager.CreateAsync(role);
        }

        await _context.SaveChangesAsync();
        var account = await _apiService.GetUserAsync("omar94");
        
        var users = new List<User>()
        {
            new()
            {
                FullName = "omar", UserName = "omar", Email = "omar@mail.com",
                CodeforcesAccount = _mapper.Map<CodeforcesAccount>(account)
            },
            new() {FullName = "ahmad", UserName = "ahmad", Email = "ahmad@mail.com"},
            new() {FullName = "ali", UserName = "ali", Email = "ali@mail.com"},
            new() {FullName = "yahya", UserName = "yahya", Email = "yahya@mail.com"},
        };
        foreach (var user in users)
        {
            user.Email = user.Email.ToLower();
            await _userManager.CreateAsync(user, "123.com.net");
            await _userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new User
        {
            UserName = "admin",
            Email = "admin@mail.com"
        };
        await _userManager.CreateAsync(admin, "123.com.net");
        await _userManager.AddToRolesAsync(admin, new[] {"Admin", "Coach"});
        await _context.SaveChangesAsync();

        var team = new Team
        {
            Name = "team1",
            Coach = await _context.Users.FindAsync(1),
        };
        var members = await _context.Users.Where(x => x.UserName != "admin")
            .Where(x => x.UserName != "omar").ToListAsync();
        var membersToAdd = members.Select(x=>new TeamUser
        {
            User = x,
            Team=team
        }).ToList();
        team.Members = membersToAdd;
        await _context.Teams.AddAsync(team);
        
        var participation = new Participation
        {
            Location = "Russia",
            Rank = 1,
            Name = "ICPC 2020",
            Year = "2020",
            TeamName = "Red Panda",
            User = await _context.Users.FindAsync(1)
        };
        await _context.Participations.AddAsync(participation);

        var group = new TrainingGroup
        {
            Name = "advanced",
            Coach = await _context.Users.FindAsync(1),
        };
        await _context.TrainingGroups.AddAsync(group);
        await _context.SaveChangesAsync();
        group = await _context.TrainingGroups.FindAsync(1);
        var trainees = new List<TrainingGroupUser>
        {
            new() {TrainingGroup = group, User = await _context.Users.FindAsync(2)},
            new() {TrainingGroup = group, User = await _context.Users.FindAsync(3)},
            new() {TrainingGroup = group, User = await _context.Users.FindAsync(1)}
        };
        group!.Students = trainees;
        await _context.SaveChangesAsync();
    }
    
    private async Task SeedProblems()
    {
        if (await _context.Problems.AnyAsync()) return;
        // context.Problems.RemoveRange(context.Problems);
        // context.Submissions.RemoveRange(context.Submissions);
        await _context.SaveChangesAsync();
        var tags = UsedTags.TagsUsed.Select(x => new Tag {Name = x}).ToList();
        _context.Tags.AddRange(tags);
        await _context.SaveChangesAsync();
        
        var problems = await _apiService.GetAllProblems();
        var problemsToAdd = problems?.
            Where(x => !x.Tags!.Contains("*special") && x.Tags.All(t=>UsedTags.TagsUsed.Contains(t)))
            .OrderBy(x=>x.ContestId)
            .Take(500)
            .Select(x=>new Problem
            {
                Name=x.Name,
                ContestId=x.ContestId,
                Index=x.Index,
                Rating=x.Rating,
                Tags = _context.Tags.Where(t=>x.Tags!.Contains(t.Name!)).ToList()
            }).AsParallel();
        
        await _context.Problems.AddRangeAsync(problemsToAdd!);
        await _context.SaveChangesAsync();
    }

    private async Task SeedSubmissions()
    {
        //var problems = await context.Problems.ToListAsync();
        var users = _context.Users.Include(u => u.CodeforcesAccount).ToList();
        foreach (var user in users)
        {
            if (await _context.Submissions.AnyAsync(x => x.Author == user)) continue;
            if (user.CodeforcesAccount?.Handle==null) continue;
            var submissions = await _apiService.GetSubmissionsAsync(user.CodeforcesAccount.Handle);
            var submissionsToAdd = submissions?
                .Where(x => x.Verdict == "OK" && !x.Problem!.Tags!.Contains("*special")
                                              && x.Problem.Tags.All(t => UsedTags.TagsUsed.Contains(t))
                                              && _context.Problems.Any(p =>
                                                  p.Index == x.Problem.Index && p.ContestId == x.Problem.ContestId))
                .Select(s => new Submission
                {
                    Author = user,
                    Problem = _context.Problems.Find(s.Problem?.ContestId, s.Problem?.Index),
                    Verdict = "OK"
                }).AsParallel().ToList();
            if(submissionsToAdd==null || submissionsToAdd.Count==0)continue;
            user.ProblemsAvg = submissionsToAdd.Select(x => x.Problem!.Rating).ToList().Average();
            await _context.AddRangeAsync(submissionsToAdd);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCodeforcesUsers()
    {
        if (await _context.CodeforceseAccounts.CountAsync() > 1) return;
        var users = await _apiService.GetSyriaUsers();
        if (users is null) return;
        var usersToAdd = users.Where(x => x.Country == "Syria").
            Where(x=>x.Handle!="omar94").
            Select(_mapper.Map<CodeforcesAccount>).Take(100).AsParallel().ToList();

        await _context.CodeforceseAccounts.AddRangeAsync(usersToAdd);
        await _context.SaveChangesAsync();
        usersToAdd = await _context.CodeforceseAccounts.ToListAsync();
        var accounts = usersToAdd.Select(x => new User
        {
            FullName = x.FirstName is not null ? x.FirstName + " " + x.LastName : x.Handle,
            Email = x.Handle + "@mail.com",
            UserName = x.Handle
        });
        foreach (var user in accounts)
        {
            user.Email = user.Email.ToLower();
            await _userManager.CreateAsync(user, "123.com.net");
            await _userManager.AddToRoleAsync(user, "Member");
        }
        await _context.SaveChangesAsync();
        var accountsAdded = await _userManager.Users.ToListAsync();
        foreach (var user in accountsAdded)
        {
            user.CodeforcesAccount =
                await _context.CodeforceseAccounts.FirstOrDefaultAsync(x => x.Handle == user.UserName);
        }
        await _context.SaveChangesAsync();
        await _context.SaveChangesAsync();
    }
}
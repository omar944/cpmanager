using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

#pragma warning disable CS8601

namespace API.Data;

public static class Seed
{
    public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager,
        AppDbContext context, CodeforcesApiService apiService, IMapper mapper)
    {
        if (await userManager.Users.AnyAsync()) return;

        var roles = new List<Role>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Coach"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        await context.SaveChangesAsync();
        //var account = await apiService.GetUserAsync("omar94");
        
        var users = new List<User>()
        {
            new()
            {
                FullName = "omar", UserName = "omar", Email = "omar@mail.com",
                //CodeforcesAccount = mapper.Map<CodeforcesAccount>(account)
            },
            new() {FullName = "ahmad", UserName = "ahmad", Email = "ahmad@mail.com"},
            new() {FullName = "ali", UserName = "ali", Email = "ali@mail.com"},
            new() {FullName = "yahya", UserName = "yahya", Email = "yahya@mail.com"},
        };

        foreach (var user in users)
        {
            user.Email = user.Email.ToLower();
            await userManager.CreateAsync(user, "123.com.net");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new User
        {
            UserName = "admin",
            Email = "admin@mail.com"
        };
        await userManager.CreateAsync(admin, "123.com.net");
        await userManager.AddToRolesAsync(admin, new[] {"Admin", "Coach"});
        await context.SaveChangesAsync();
        
        var team = new Team
        {
            Name = "team1",
            Coach = await context.Users.FindAsync(1),
        };
        var members = await context.Users.Where(x => x.UserName != "admin").ToListAsync();
        var membersToAdd = members.Select(x=>new TeamUser
        {
            User = x,
            Team=team
        }).ToList();
        team.Members = membersToAdd;
        await context.Teams.AddAsync(team);
        
        var participation = new Participation
        {
            Location = "Russia",
            Rank = 1,
            Name = "ICPC 2020",
            Year = "2020",
            TeamName = "Red Panda",
            User = await context.Users.FindAsync(1)
        };
        await context.Participations.AddAsync(participation);

        var group = new TrainingGroup
        {
            Name = "advanced",
            Coach = await context.Users.FindAsync(1),
        };
        await context.TrainingGroups.AddAsync(group);
        await context.SaveChangesAsync();
        group = await context.TrainingGroups.FindAsync(1);
        var trainees = new List<TrainingGroupUser>
        {
            new() {TrainingGroup = group, User = await context.Users.FindAsync(2)},
            new() {TrainingGroup = group, User = await context.Users.FindAsync(3)},
            new() {TrainingGroup = group, User = await context.Users.FindAsync(1)}
        };
        group!.Students = trainees;
    }
    
    public static async Task SeedProblems(AppDbContext context, CodeforcesApiService apiService)
    {
        if (await context.Problems.AnyAsync()) return;
        //context.Problems.RemoveRange(context.Problems);
        // context.Submissions.RemoveRange(context.Submissions);
        await context.SaveChangesAsync();
        var tags = UsedTags.TagsUsed.Select(x => new Tag {Name = x}).ToList();
        context.Tags.AddRange(tags);
        await context.SaveChangesAsync();
        
        var problems = await apiService.GetAllProblems();
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
                Tags = context.Tags.Where(t=>x.Tags!.Contains(t.Name!)).ToList()
            }).AsParallel();
        
        await context.Problems.AddRangeAsync(problemsToAdd!);
        await context.SaveChangesAsync();
    }

    public static async Task SeedSubmissions(AppDbContext context, CodeforcesApiService apiService)
    {
        if (await context.Submissions.AnyAsync()) return;
        var problems = await context.Problems.ToListAsync();
        var author = await context.Users.FindAsync(1);

        var users = new List<string> {"omar94"};

        foreach (var user in users)
        {
            var submissions = await apiService.GetSubmissionsAsync(user);
            var submissionsToAdd = submissions?
                .Where(x => x.Verdict == "OK" && !x.Problem!.Tags!.Contains("*special")
                                              && x.Problem.Tags.All(t => UsedTags.TagsUsed.Contains(t))
                                              && problems.Any(p =>
                                                  p.Index == x.Problem.Index && p.ContestId == x.Problem.ContestId))
                .Select(s => new Submission
                {
                    Author = author,
                    Problem = context.Problems.Find(s.Problem?.ContestId, s.Problem?.Index),
                    Verdict = "OK"
                }).AsParallel();
            await context.AddRangeAsync(submissionsToAdd!);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedCodeforcesUsers(AppDbContext context, CodeforcesApiService apiService
        , IMapper mapper,UserManager<User> userManager)
    {
        if (await context.CodeforceseAccounts.CountAsync() > 1) return;
        var users = await apiService.GetSyriaUsers();
        //var file = File.OpenText(@"D:\response.json").BaseStream;
        // var users = JsonSerializer
        //     .Deserialize<CodeforcesApiResult<List<CodeforcesAccountDto>>>(file,
        //         new JsonSerializerOptions {PropertyNameCaseInsensitive = true})?.Result;
        if (users is null) return;
        var usersToAdd = users.Where(x => x.Country == "Syria" && x.Handle!="omar94").
            Select(mapper.Map<CodeforcesAccount>).Take(100).AsParallel().ToList();

        await context.CodeforceseAccounts.AddRangeAsync(usersToAdd);
        await context.SaveChangesAsync();
        usersToAdd = await context.CodeforceseAccounts.ToListAsync();
        var accounts = usersToAdd.Select(x => new User
        {
            FullName = x.FirstName is not null ? x.FirstName + " " + x.LastName : x.Handle,
            Email = x.Handle + "@mail.com",
            UserName = x.Handle
        });
        foreach (var user in accounts)
        {
            user.Email = user.Email.ToLower();
            await userManager.CreateAsync(user, "123.com.net");
            await userManager.AddToRoleAsync(user, "Member");
        }
        await context.SaveChangesAsync();
        var accountsAdded = await userManager.Users.ToListAsync();
        foreach (var user in accountsAdded)
        {
            user.CodeforcesAccount =
                await context.CodeforceseAccounts.FirstOrDefaultAsync(x => x.Handle == user.UserName);
        }

        await context.SaveChangesAsync();
    }
}
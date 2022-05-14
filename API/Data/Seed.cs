using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;
#pragma warning disable CS8601

namespace API.Data;

public static class Seed
{
    public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager,
        AppDbContext context, CodeforcesApiService apiService,IMapper mapper)
    {
        if (await userManager.Users.AnyAsync()) return;

        // var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        // var users = JsonSerializer.Deserialize<List<User>>(userData);
        // if (users == null) return;

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

        var account = await apiService.GetUserAsync("omar94");
        
        var users = new List<User>()
        {
            new() {UserName = "omar",CodeforcesAccount = mapper.Map<CodeforcesAccount>(account)},
            new() {UserName = "ahmad"},
            new() {UserName = "ali"},
            new() {UserName = "yahya"},
        };
        
        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();
            await userManager.CreateAsync(user, "123.com.net");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new User
        {
            UserName = "admin"
        };
        await userManager.CreateAsync(admin, "123.com.net");
        await userManager.AddToRolesAsync(admin, new[] {"Admin", "Coach"});

        var team = new Team
        {
            Name = "team1",
            Members = await context.Users.Where(x => x.UserName != "admin").ToListAsync()
        };
        await context.Teams.AddAsync(team);
        var participation = new Participation
        {
            Location = "Russia",
            Rank = 1,
            Name = "ICPC 2020",
            Year = "2020",
            Team = team,
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

        var problems = await apiService.GetAllProblems();
        foreach (var dto in problems)
        {
            foreach (var tag in dto.Tags)
            {
                if (await context.Tags.FirstOrDefaultAsync(t => t.Name == tag) == null)
                {
                    context.Tags.Add(new Tag {Name = tag});
                }
            }
            await context.SaveChangesAsync();
            
            context.Problems.Add(new Problem
            {
                Name=dto.Name,
                ContestId=dto.ContestId,
                Index=dto.Index,
                Rating=dto.Rating,
                Tags = await context.Tags.Where(t=>dto.Tags.Contains(t.Name)).ToListAsync()
            });
        }

        var submissions = await apiService.GetSubmissionsAsync("omar94",200);
        foreach (var dto in submissions)
        {
            if (dto.Verdict != "OK") continue;
            
            var propblemDto = dto.Problem;
            if (await context.Problems.Where(x => x.Name == propblemDto.Name).AnyAsync() == false)
            {
                foreach (var tag in propblemDto.Tags)
                {
                    if (await context.Tags.FirstOrDefaultAsync(t => t.Name == tag) == null)
                    {
                        context.Tags.Add(new Tag {Name = tag});
                    }
                }

                await context.SaveChangesAsync();
                context.Problems.Add(new Problem
                {
                    Name = propblemDto.Name,
                    ContestId = propblemDto.ContestId,
                    Index = propblemDto.Index,
                    Rating = propblemDto.Rating,
                    Tags = await context.Tags.Where(t => propblemDto.Tags.Contains(t.Name)).ToListAsync()
                });
            }
            await context.SaveChangesAsync();
            context.Submissions.Add(new Submission
            {
                Author=await context.Users.FindAsync(1),
                Problem=await context.Problems.Where(x=>x.Name==propblemDto.Name).FirstOrDefaultAsync(),
                Verdict="OK"
            });
        }
        await context.SaveChangesAsync();
    }
}
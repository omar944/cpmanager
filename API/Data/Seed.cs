﻿using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

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
            new() {TrainingGroup = group, User = await context.Users.FindAsync(3)}
        };
        group.Students = trainees;

        await context.SaveChangesAsync();
    }
}
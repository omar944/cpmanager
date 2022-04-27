using Entities.App;
using Entities.Codeforces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : IdentityDbContext<User, Role, int
    , IdentityUserClaim<int>, UserRole
    , IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    //App
    public DbSet<Team>? Teams { get; set; }
    public DbSet<TrainingGroup>? TrainingGroups { get; set; }
    public DbSet<Participation>? Participations { get; set; }
    public DbSet<DailyTask>? DailyTasks { get; set; }
    public DbSet<Blog>? Blogs { get; set; }

    //codeforces
    public DbSet<Problem>? Problems { get; set; }
    public DbSet<Submission>? Submissions { get; set; }
    public DbSet<CodeforcesAccount> CodeforceseAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.Entity<Role>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

        
        builder.Entity<User>()
            .HasOne(u => u.CodeforcesAccount)
            .WithOne(x => x.Owner)
            .HasForeignKey<CodeforcesAccount>(c => c.CodeforcesAccountForeignKey);

        //Training groups
        builder.Entity<User>()
            .HasMany(u => u.TrainingGroups)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId);

        builder.Entity<TrainingGroup>()
            .HasMany(u => u.Students)
            .WithOne(u => u.TrainingGroup)
            .HasForeignKey(u => u.TrainingGroupId);

        //Coach
        builder.Entity<User>()
            .HasMany(u => u.TeachingGroups)
            .WithOne(u => u.Coach)
            .HasForeignKey(u => u.CoachId);

    }
}



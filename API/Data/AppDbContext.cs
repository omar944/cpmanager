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
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TrainingGroup> TrainingGroups { get; set; } = null!;
    public DbSet<Participation> Participations { get; set; } = null!;
    public DbSet<DailyTask> DailyTasks { get; set; } = null!;
    
    public DbSet<Blog> Blogs { get; set; } = null!;

    //codeforces
    public DbSet<Problem> Problems { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<CodeforcesAccount> CodeforceseAccounts { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    
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
        
        //Blogs
        builder.Entity<User>()
            .HasMany(ub => ub.Blogs)
            .WithOne(blog => blog.Author)
            .HasForeignKey(b => b.AuthorId);


        builder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.Author)
            .HasForeignKey(s => s.UserId);

        builder.Entity<Problem>()
            .HasMany(p => p.Submissions)
            .WithOne(s => s.Problem)
            .HasForeignKey(s => new {s.ProblemContestId, s.ProblemIndex});

        builder.Entity<Problem>()
            .HasKey(p => new {p.ContestId, p.Index});
    }
}



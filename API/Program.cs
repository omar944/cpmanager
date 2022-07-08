using API.Data;
using API.Extensions;
using CodeforcesTool.Services;
using Entities.App;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(x=>x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    var dbContext = services.GetRequiredService<AppDbContext>();
    var apiService = services.GetRequiredService<CodeforcesApiService>();
    var mapper=services.GetRequiredService<IMapper>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager,dbContext,apiService,mapper);
    await Seed.SeedProblems(dbContext,apiService);
    await Seed.SeedSubmissions(dbContext,apiService);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "error in migration");
}

await app.RunAsync();
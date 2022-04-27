using System.Text.Json.Serialization;
using API.Data;
using API.Interfaces;
using API.Services;
using CodeforcesTool.Services;

namespace API.Extensions;

public static class AppServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
    {
        services.AddScoped<ITokenService,TokenService>();
        
        services.AddAutoMapper(typeof(AutoMapperConfiguration).Assembly);

        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data source=testApp.db");
        });
        
        services.AddHttpClient<CodeforcesApiService>();

        services.AddMvc().AddJsonOptions(options =>
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);
        
        return services;
    }
}
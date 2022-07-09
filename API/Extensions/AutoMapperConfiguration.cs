using API.Models;
using CodeforcesTool.Models;
using Entities.App;
using Entities.Codeforces;

namespace API.Extensions;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    { 
        // CreateMap<MemberUpdateDto,AppUser>().
        //     ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserBlogDto>();
        CreateMap<Team, TeamDto>();

        CreateMap<User, TeamUserDto>();
        
        CreateMap<Blog, BlogDto>();

        CreateMap<Participation, ParticipationDto>();

        CreateMap<TrainingGroupUser, TrainingGroupDto>().IncludeMembers(u=>u.TrainingGroup);
        CreateMap<TrainingGroup, TrainingGroupDto>();
        CreateMap<TrainingGroupUser, int>().ConvertUsing(u => u.UserId);
        
        CreateMap<TeamUser, int>().ConvertUsing(u => u.UserId);

        CreateMap<User, int>().ConvertUsing(u => u.Id);
        CreateMap<User, string>().ConvertUsing(u => u.UserName);
        CreateMap<CodeforcesAccount, string>().ConvertUsing(u => u.Handle!);

        CreateMap<CodeforcesAccountDto, CodeforcesAccount>();

        CreateMap<Problem, ProblemDto>();
        CreateMap<Tag, string>().ConvertUsing(t=>t.Name!);

        CreateMap<DailyTask, DailyTaskDto>();

        CreateMap<UserRole, string>().ConvertUsing(x=>x.Role.Name);
    }
}
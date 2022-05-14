using API.Models;
using CodeforcesTool.Models;
using Entities.App;
using Entities.Codeforces;
using ProblemDto = API.Models.ProblemDto;

namespace API.Extensions;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    { 
        // CreateMap<MemberUpdateDto,AppUser>().
        //     ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<Team, TeamDto>();
        
        CreateMap<Blog, BlogDto>();

        CreateMap<Participation, ParticipationDto>();

        CreateMap<TrainingGroupUser, TrainingGroupDto>().IncludeMembers(u=>u.TrainingGroup);
        CreateMap<TrainingGroup, TrainingGroupDto>();
        CreateMap<TrainingGroupUser, string>().ConvertUsing(u => u.User!.UserName);

        CreateMap<User, string>().ConvertUsing(u => u.UserName);
        CreateMap<CodeforcesAccount, string>().ConvertUsing(u => u.Handle ?? "");

        CreateMap<CodeforcesAccountDto, CodeforcesAccount>();

        CreateMap<Problem, ProblemDto>();
        CreateMap<Tag, string>().ConvertUsing(t=>t.Name!);


        CreateMap<DailyTask, DailyTaskDto>();
    }
}
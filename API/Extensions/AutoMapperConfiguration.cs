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
        
        CreateMap<UserUpdateDto,User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserBlogDto>();
        CreateMap<Team, TeamDto>();

        CreateMap<User, TeamUserDto>();
        
        
        CreateMap<Blog, BlogDto>();
        
        CreateMap<TrainingSession, TrainingSessionDto>().ReverseMap();
        
        CreateMap<Participation, ParticipationDto>();

        CreateMap<TrainingGroupUser, TrainingGroupDto>().IncludeMembers(u=>u.TrainingGroup);
        CreateMap<User, TrainingGroupStudentDto>();
        CreateMap<TrainingGroup, TrainingGroupDto>();
        //CreateMap<TrainingGroupUser, int>().ConvertUsing(u => u.UserId);
        CreateMap<TrainingGroupUser, TrainingGroupStudentDto>().IncludeMembers(u=>u.User)
            .ForMember(x=>x.Id,opt=>opt.MapFrom(src=>src.User!.Id));
        // CreateMap<TrainingGroup, TrainingGroupStudentDto>();
        
        CreateMap<TeamUser, int>().ConvertUsing(u => u.UserId);

        CreateMap<User, int>().ConvertUsing(u => u.Id);
        CreateMap<User, string>().ConvertUsing(u => u.UserName);
        CreateMap<CodeforcesAccount, string>().ConvertUsing(u => u.Handle!);

        CreateMap<CodeforcesAccountDto, CodeforcesAccount>();

        CreateMap<Problem, ProblemDto>();
        CreateMap<Tag, string>().ConvertUsing(t=>t.Name!);

        CreateMap<DailyTask, DailyTaskDto>();
        CreateMap<DailyTask, UserTaskStatsDto>();

        CreateMap<UserRole, string>().ConvertUsing(x=>x.Role.Name);
        
        CreateMap<TeamUser,TeamDto>().IncludeMembers(u=>u.Team);
        CreateMap<TeamUser,TeamUserDto>().IncludeMembers(u=>u.User);

        CreateMap<ParticipationCreateDto, Participation>();
        CreateMap<Participation,ParticipationDto>();
    }
}
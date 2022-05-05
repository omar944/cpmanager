using API.Models;
using CodeforcesTool.Models;
using Entities.App;
using Entities.Codeforces;

namespace API.Extensions;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        // CreateMap<User, MemberDto>()
        //     .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
        //     .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
        //
        // CreateMap<MemberUpdateDto,AppUser>().
        //     ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        //
        // CreateMap<Message, MessageDto>()
        //     .ForMember(dest => dest.SenderPhotoUrl,
        //         opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
        //     .ForMember(dest => dest.RecipientPhotoUrl,
        //         opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Blog, BlogDto>();

        CreateMap<Blog, BlogCreateDto>();

        CreateMap<Participation, ParticipationDto>();

        CreateMap<TrainingGroupUser, TrainingGroupDto>().IncludeMembers(u=>u.TrainingGroup);
        CreateMap<TrainingGroup, TrainingGroupDto>();
        CreateMap<TrainingGroupUser, string>().ConvertUsing(u => u.User.UserName);

        CreateMap<User, string>().ConvertUsing(u => u.UserName);
        CreateMap<CodeforcesAccount, string>().ConvertUsing(u => u.Handle ?? "");

        CreateMap<CodeforcesUserDto, CodeforcesAccount>();
    }
}
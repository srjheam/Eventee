using AutoMapper;
using Eventee.Api.Controllers.Dtos;
using Eventee.Api.Models;

namespace Eventee.Api.Profiles;

public class EventeeProfile : Profile
{
    public EventeeProfile()
    {
        CreateMap<GetTogether, GetTogetherDto>()
            .ForMember(dest => dest.HosterId, opt => opt.MapFrom(src => src.Hoster.Id));

        CreateMap<User, UserDto>();
    }
}

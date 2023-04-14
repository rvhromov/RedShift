using AutoMapper;
using RedShift.Application.Dtos.Users;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.MapperProfiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserReadModel, UserDto>(MemberList.None);
    }
}

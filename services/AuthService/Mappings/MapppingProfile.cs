using AutoMapper;
using AuthService.Models.Domain;
using AuthService.Models.DTOs;

namespace AuthService.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}

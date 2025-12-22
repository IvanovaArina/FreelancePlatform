using AutoMapper;
using ProfileService.Models.Domain;
using ProfileService.Models.DTOs;

namespace ProfileService.Mappings;

public class ProfileMappingProfile : Profile
{
    public ProfileMappingProfile()
    {
        CreateMap<FreelancerProfile, FreelancerProfileDto>();
    }
}

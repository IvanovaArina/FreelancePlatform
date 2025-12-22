using AutoMapper;
using FreelancePlatformConnect.Api.Models.Domain;
using FreelancePlatformConnect.Api.Models.DTOs;

namespace FreelancePlatformConnect.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<PortfolioItem, PortfolioItemDto>();
        CreateMap<CreatePortfolioItemRequest, PortfolioItem>();
        CreateMap<UpdatePortfolioItemRequest, PortfolioItem>();
        CreateMap<Subscription, SubscriptionDto>();
    }
}
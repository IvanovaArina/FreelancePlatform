using AutoMapper;
using PortfolioService.Models.Domain;
using PortfolioService.Models.DTOs;

namespace PortfolioService.Mappings
{
    public class PortfolioMappingProfile : Profile
    {
        public PortfolioMappingProfile()
        {
            CreateMap<PortfolioItem, PortfolioItemDto>();
            CreateMap<CreatePortfolioItemRequest, PortfolioItem>();
            CreateMap<UpdatePortfolioItemRequest, PortfolioItem>();
        }
    }
}

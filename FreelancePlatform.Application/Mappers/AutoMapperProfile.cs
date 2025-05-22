using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace FreelancePlatform.Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
       { CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<FreelancerProfile, FreelancerProfileDto>().ReverseMap();
        CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}

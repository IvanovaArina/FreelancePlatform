using AutoMapper;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Domain.Interfaces;
using FreelancePlatform.Application.DTOs;
using System.Threading.Tasks;
using FreelancePlatform.Infrastructure.Repository;

namespace FreelancePlatform.Application.Services
{
    public class ProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public async Task<FreelancerProfileDto> CreateProfileAsync(string freelancerId, string name, List<string> skills, List<string> portfolio, List<string> reviews)
        {
            var profile = new FreelancerProfile
            {
                UserId = freelancerId,
                Name = name,
                Skills = skills ?? new List<string>(),
                Portfolio = portfolio ?? new List<string>(),
                Reviews = reviews ?? new List<string>()
            };

            await _profileRepository.AddAsync(profile);
            return _mapper.Map<FreelancerProfileDto>(profile);
        }

        public async Task<FreelancerProfileDto> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _profileRepository.GetProfileByUserIdAsync(userId);
            if (profile == null)
                throw new InvalidOperationException("Profile not found");
            return _mapper.Map<FreelancerProfileDto>(profile);
        }




    }
}
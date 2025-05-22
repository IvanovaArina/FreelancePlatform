using AutoMapper;
using FreelancePlatform.Application.DTOs;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Repository;

// Этот класс содержит бизнес-логику для управления профилями фрилансеров: создание профиля.
// Мы убрали паттерн Builder, заменив его на прямое создание объекта FreelancerProfile.
// Проверяет, что пользователь — фрилансер, перед созданием профиля. Почему только создание?
// Обновление происходит через другие сервисы, а удаление не требуется.

namespace FreelancePlatform.Application.Services
{
    public class ProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IUserRepository userRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<FreelancerProfileDto> CreateProfileAsync(string userId, string name, string skill, string portfolioItem, string review)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Role != "Freelancer")
                throw new InvalidOperationException("Only freelancers can create profiles");

            var profile = new FreelancerProfile
            {
                UserId = userId,
                Name = name,
                Skills = new List<string> { skill },
                Portfolio = new List<string> { portfolioItem },
                Reviews = new List<string> { review }
            };
            await _profileRepository.AddAsync(profile);
            return _mapper.Map<FreelancerProfileDto>(profile);
        }
    }
}

using ProfileService.Models.Domain;

namespace ProfileService.Repositories.Interfaces;

public interface IFreelancerProfileRepository
{
    Task<FreelancerProfile?> GetByUserIdAsync(int userId);
    Task AddAsync(FreelancerProfile profile);
    Task UpdateAsync(FreelancerProfile profile);
}

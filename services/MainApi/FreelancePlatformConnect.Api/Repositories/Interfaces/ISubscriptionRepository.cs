using FreelancePlatformConnect.Api.Models.Domain;

namespace FreelancePlatformConnect.Api.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<List<Subscription>> GetByFreelancerIdAsync(int freelancerId);
        Task<Subscription?> GetByIdAsync(int id);
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
        Task DeleteAsync(Subscription subscription);
    }
}


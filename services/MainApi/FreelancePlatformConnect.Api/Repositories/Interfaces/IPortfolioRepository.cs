using FreelancePlatformConnect.Api.Models.Domain;

namespace FreelancePlatformConnect.Api.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<PortfolioItem>> GetByFreelancerIdAsync(int freelancerId);
        Task<PortfolioItem?> GetByIdAsync(int id);
        Task AddAsync(PortfolioItem item);
        Task UpdateAsync(PortfolioItem item);
        Task DeleteAsync(PortfolioItem item);
    }
}

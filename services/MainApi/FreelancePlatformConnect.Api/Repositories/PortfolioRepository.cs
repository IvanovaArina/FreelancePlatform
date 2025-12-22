using FreelancePlatformConnect.Api.Data;
using FreelancePlatformConnect.Api.Models.Domain;
using FreelancePlatformConnect.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelancePlatformConnect.Api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _db;
        public PortfolioRepository(AppDbContext db) => _db = db;

        public async Task<List<PortfolioItem>> GetByFreelancerIdAsync(int freelancerId)
            => await _db.PortfolioItems.Where(p => p.FreelancerId == freelancerId).ToListAsync();

        public async Task<PortfolioItem?> GetByIdAsync(int id)
            => await _db.PortfolioItems.FindAsync(id);

        public async Task AddAsync(PortfolioItem item)
        {
            _db.PortfolioItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(PortfolioItem item)
        {
            _db.PortfolioItems.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(PortfolioItem item)
        {
            _db.PortfolioItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
}

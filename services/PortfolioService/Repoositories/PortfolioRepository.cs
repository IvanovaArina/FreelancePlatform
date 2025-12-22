using PortfolioService.Data;
using PortfolioService.Models.Domain;
using PortfolioService.Repoositories.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

namespace PortfolioService.Repoositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly PortfolioDbContext _db;
        public PortfolioRepository(PortfolioDbContext db) => _db = db;

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

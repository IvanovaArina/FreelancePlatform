using AutoMapper;
using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{

    public class PGProfileRepository : IProfileRepository
    {
        private readonly FreelanceDbContext _context;

        public PGProfileRepository(FreelanceDbContext context)
        {
            _context = context;
        }

        public async Task<FreelancerProfile> GetProfileByUserIdAsync(string userId)
        {
            return await _context.FreelancerProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(FreelancerProfile profile)
        {
            await _context.FreelancerProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

     
    }
}
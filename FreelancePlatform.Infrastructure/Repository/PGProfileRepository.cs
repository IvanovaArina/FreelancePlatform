using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task AddAsync(FreelancerProfile profile)
        {
            await _context.FreelancerProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }
    }
}

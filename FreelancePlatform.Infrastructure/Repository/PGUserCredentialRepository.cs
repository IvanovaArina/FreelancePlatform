using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{
    public class PGUserCredentialRepository : IUserCredentialsRepository
    {
        private readonly FreelanceDbContext _context;

        public PGUserCredentialRepository(FreelanceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(UserCredentials credentials)
        {
            await _context.UserCredentials.AddAsync(credentials);
            await _context.SaveChangesAsync();
        }

        public async Task<UserCredentials> GetByUserIdAsync(string userId)
        {
            return await _context.UserCredentials.FindAsync(userId);    
        }
    }
}

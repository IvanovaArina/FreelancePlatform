using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{
    public class PGUserRepository : IUserRepository
    {
        private readonly FreelanceDbContext _context;

        public PGUserRepository(FreelanceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>u.Email == email);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}

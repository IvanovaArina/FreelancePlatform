using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{
    public class PGTransactionRepository : ITransactionRepository
    {
        private readonly FreelanceDbContext _context;

        public PGTransactionRepository(FreelanceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}

using FreelancePlatform.Domain.Entities;
using FreelancePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace FreelancePlatform.Infrastructure.Repository
{
    public class PGOrderRepository : IOrderRepository
    {
        private readonly FreelanceDbContext _context;

        public PGOrderRepository(FreelanceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

        }

        public async Task<Order> GetByIdAsync(string id)
        {
           return  await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Order>> GetActiveOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetFreeOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.FreelancerId == null)
                .ToListAsync();
        }
    }
}

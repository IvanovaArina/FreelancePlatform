using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(string id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task<List<Order>> GetActiveOrdersAsync();
        Task<List<Order>> GetFreeOrdersAsync();  

    }
}

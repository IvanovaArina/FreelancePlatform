using FreelancePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Repository
{
    public interface IUserCredentialsRepository
    {
        Task AddAsync(UserCredentials credentials);
        Task<UserCredentials> GetByUserIdAsync(string userId);
    }
}

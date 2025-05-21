using FreelancePlatform.Domain.Entities;


namespace FreelancePlatform.Infrastructure.Repository
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
    }
}

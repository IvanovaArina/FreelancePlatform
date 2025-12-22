using FreelancePlatformConnect.Api.Data;
using FreelancePlatformConnect.Api.Models.Domain;
using FreelancePlatformConnect.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelancePlatformConnect.Api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _db;
    public SubscriptionRepository(AppDbContext db) => _db = db;

    public async Task<List<Subscription>> GetByFreelancerIdAsync(int freelancerId)
        => await _db.Subscriptions
            .Where(s => s.FreelancerId == freelancerId)
            .OrderBy(s => s.Price)
            .ToListAsync();

    public async Task<Subscription?> GetByIdAsync(int id)
        => await _db.Subscriptions.FindAsync(id);

    public async Task AddAsync(Subscription subscription)
    {
        _db.Subscriptions.Add(subscription);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _db.Subscriptions.Update(subscription);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Subscription subscription)
    {
        _db.Subscriptions.Remove(subscription);
        await _db.SaveChangesAsync();
    }
}           
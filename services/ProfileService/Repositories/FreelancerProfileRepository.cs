using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Models.Domain;
using ProfileService.Repositories.Interfaces;

namespace ProfileService.Repositories;

public class FreelancerProfileRepository : IFreelancerProfileRepository
{
    private readonly ProfileDbContext _db;

    public FreelancerProfileRepository(ProfileDbContext db) => _db = db;

    public Task<FreelancerProfile?> GetByUserIdAsync(int userId)
        => _db.FreelancerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task AddAsync(FreelancerProfile profile)
    {
        _db.FreelancerProfiles.Add(profile);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(FreelancerProfile profile)
    {
        _db.FreelancerProfiles.Update(profile);
        await _db.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using ProfileService.Models.Domain;

namespace ProfileService.Data;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

    public DbSet<FreelancerProfile> FreelancerProfiles => Set<FreelancerProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FreelancerProfile>()
            .HasKey(p => p.UserId);

        modelBuilder.Entity<FreelancerProfile>()
            .Property(p => p.Skills)
            .HasColumnType("jsonb");
    }
}

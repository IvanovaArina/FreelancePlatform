using Microsoft.EntityFrameworkCore;
using PortfolioService.Models.Domain;

namespace PortfolioService.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
            : base(options) { }

        public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortfolioItem>()
                .HasKey(p => p.Id);
        }
    }

}

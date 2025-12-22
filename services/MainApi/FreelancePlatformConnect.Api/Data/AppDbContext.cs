using FreelancePlatformConnect.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FreelancePlatformConnect.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<FreelancerProfile> FreelancerProfiles => Set<FreelancerProfile>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1-к-1 связь с shared primary key
        modelBuilder.Entity<FreelancerProfile>()
            .HasKey(p => p.UserId);

        modelBuilder.Entity<FreelancerProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.FreelancerProfile)
            .HasForeignKey<FreelancerProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        // JSONB для Skills
        modelBuilder.Entity<FreelancerProfile>()
            .Property(p => p.Skills)
            .HasColumnType("jsonb");

        modelBuilder.Entity<PortfolioItem>()
            .HasOne<FreelancerProfile>()
            .WithMany()
            .HasForeignKey(p => p.FreelancerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Subscription>()
            .Property(s => s.WhatIncludes)
            .HasColumnType("jsonb");

        modelBuilder.Entity<Subscription>()
            .HasOne<FreelancerProfile>()
            .WithMany()
            .HasForeignKey(s => s.FreelancerId)
            .OnDelete(DeleteBehavior.Cascade); // при удалении профиля — удаляются подписки
    }
}
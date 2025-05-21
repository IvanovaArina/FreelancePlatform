using FreelancePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Infrastructure.Data
{
    public class FreelanceDbContext : DbContext
    {
        public FreelanceDbContext(DbContextOptions<FreelanceDbContext> options) : base(options)
        {

        }
        
        //tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FreelancerProfile> FreelancerProfiles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }

        // Это переопределённый метод, в котором ты настраиваешь модель базы данных вручную,
        // если нужно больше гибкости, чем дают атрибуты [Key], [ForeignKey] и т.п.
        // Он вызывается EF Core один раз при создании модели — например, при миграциях или запуске приложения.
        // Fluent API - это способ настройки модели базы данных с помощью кода, используя цепочку методов

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<FreelancerProfile>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<UserCredentials>().HasKey(uc => uc.UserId);

            modelBuilder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.FreelancerId);

            modelBuilder.Entity<FreelancerProfile>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<UserCredentials>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<UserCredentials>(uc => uc.UserId);


        }


    }
}

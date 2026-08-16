using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.ValueObjects;
using TC1.RepairShop.Domain.Registration;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data
{
    public class RepairShopDbContext : DbContext
    {
        public RepairShopDbContext(DbContextOptions<RepairShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Quote> Quotes { get; set; } = null!;
        public DbSet<ServiceOrder> ServiceOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PartConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceOrderConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuoteConfiguration());
        }

        // DbContext registration helper is provided in Extensions/ServiceCollectionExtensions
    }
}

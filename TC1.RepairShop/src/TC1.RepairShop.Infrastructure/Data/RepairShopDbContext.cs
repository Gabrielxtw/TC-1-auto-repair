using MediatR;
using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;


namespace TC1.RepairShop.Infrastructure.Data
{
    public class RepairShopDbContext : DbContext
    {
        private readonly IPublisher _publisher;
        public RepairShopDbContext(DbContextOptions<RepairShopDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Sets default precision and scale for every decimal property across your entire model
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Quote> Quotes { get; set; } = null!;
        public DbSet<ServiceOrder> ServiceOrders { get; set; } = null!;
        public DbSet<ServiceOrderPart> ServiceOrderParts { get; set; } = null!;
        public DbSet<ServiceOrderService> ServiceOrderServices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PartConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceOrderConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceOrderPartConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.QuoteConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ServiceOrderServiceConfiguration());

        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    return entity.DequeueDomainEvents();
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }

    }
}

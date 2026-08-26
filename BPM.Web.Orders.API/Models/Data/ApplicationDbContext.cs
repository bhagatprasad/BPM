using BPM.Web.Orders.API.Models.Entities;
using BPM.Web.Orders.API.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPM.Web.Orders.API.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<PurchaseOrderApproval> PurchaseOrderApprovals { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure DateTime handling for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        // Add value converter to ensure UTC
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToDatabaseUtc(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        // Add value converter to ensure UTC
                        property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(
                            v => v.ToDatabaseUtc(),
                            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null
                        ));
                    }
                }
            }

            // Additional configuration for PurchaseOrder if needed
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                // You can add additional fluent configuration here if needed
                // But your Data Annotations should handle most of it
            });
            
        }

        public override int SaveChanges()
        {
            // Ensure all DateTime properties are UTC before saving
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(DateTime))
                    {
                        var value = (DateTime)property.GetValue(entry.Entity);
                        property.SetValue(entry.Entity, value.ToDatabaseUtc());
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)property.GetValue(entry.Entity);
                        property.SetValue(entry.Entity, value.ToDatabaseUtc());
                    }
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Ensure all DateTime properties are UTC before saving
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(DateTime))
                    {
                        var value = (DateTime)property.GetValue(entry.Entity);
                        property.SetValue(entry.Entity, value.ToDatabaseUtc());
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)property.GetValue(entry.Entity);
                        property.SetValue(entry.Entity, value.ToDatabaseUtc());
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}

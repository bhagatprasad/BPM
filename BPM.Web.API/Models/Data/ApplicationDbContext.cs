using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPM.Web.API.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugForm> DrugForms { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<DrugCategory> DrugCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<DrugUom> DrugUoms { get; set; }
        public DbSet<PackagingMaster> PackagingMasters { get; set; }
        public DbSet<DrugPackaging> DrugPackagings { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistorys { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<BatchMaster> BatchMasters { get; set; }
        public DbSet<SupplierDiscount> SupplierDiscounts { get; set; }
        public DbSet<VolumeDiscountTier> VolumeDiscountTiers { get; set; }
        public DbSet<PromotionalOffer> PromotionalOffers { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<PurchaseOrderApproval> PurchaseOrderApprovals { get; set; }



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
            // User -> Distributor relationship
            modelBuilder.Entity<User>()
               .HasOne(u => u.Distributor)
               .WithMany()
               .HasForeignKey(u => u.DistributorId)
               .HasPrincipalKey(d => d.DistributorId)
               .OnDelete(DeleteBehavior.Restrict);
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
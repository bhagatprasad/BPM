using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public DbSet<DrugCategory>DrugCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem>PurchaseOrderItems { get; set; }
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
        public DbSet<Billing>Billings { get; set; }
        public DbSet<Invoice>Invoices { get; set; }




    }
}


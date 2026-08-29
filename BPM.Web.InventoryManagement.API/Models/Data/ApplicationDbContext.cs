using BPM.Web.InventoryManagement.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.InventoryManagement.API.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
    }
}

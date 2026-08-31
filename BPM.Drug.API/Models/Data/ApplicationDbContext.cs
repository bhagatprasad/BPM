using BPM.Web.Drug.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DrugEntity = BPM.Web.Drug.API.Models.Entities.Drug;

namespace BPM.Web.Drug.API.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DrugEntity> Drugs { get; set; }

        public DbSet<DrugCategory> DrugCategories { get; set; }

        public DbSet<DrugFormEntity> DrugForms { get; set; }

        public DbSet<DrugUom> DrugUoms { get; set; }

        public DbSet<DrugPackaging> DrugPackagings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(
                            new ValueConverter<DateTime, DateTime>(
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                            ));
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(
                            new ValueConverter<DateTime?, DateTime?>(
                                v => v.HasValue
                                    ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                                    : (DateTime?)null,
                                v => v.HasValue
                                    ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                                    : (DateTime?)null
                            ));
                    }
                }
            }

            modelBuilder.Entity<DrugEntity>()
           .HasOne(d => d.DrugForm)
           .WithMany(f => f.Drugs)
           .HasForeignKey(d => d.FormId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DrugUom>()
            .HasOne(u => u.Drug)
            .WithMany(d => d.DrugUoms)
            .HasForeignKey(u => u.DrugId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DrugPackaging>()
            .HasOne(p => p.Drug)
            .WithMany(d => d.DrugPackagings)
            .HasForeignKey(p => p.DrugId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DrugPackaging>()
            .HasOne(p => p.PackageUom)
            .WithMany()
            .HasForeignKey(p => p.PackageUomId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DrugPackaging>()
           .HasOne(p => p.ContainsUom)
           .WithMany()
           .HasForeignKey(p => p.ContainsUomId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DrugUom>()
            .HasOne(u => u.ParentUom)
            .WithMany(u=>u.ChildUoms)
            .HasForeignKey(u => u.ParentUomId)
            .OnDelete(DeleteBehavior.Restrict);

        }


    }
}

using HardwareScrapper.Domain.Abstractions;
using HardwareScrapper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HardwareScrapper.DataAccess.Contexts
{
    public class HardwareDbContext : DbContext
    {
        public HardwareDbContext(DbContextOptions<HardwareDbContext> options) : base(options)
        {
        }

        public DbSet<HardwareComponent> HardwareComponents { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ScrapingSource> ScrapingSources { get; set; }
        public DbSet<ScrapingLog> ScrapingLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table-per-hierarchy (TPH) inheritance
            modelBuilder.Entity<HardwareComponent>()
                .HasDiscriminator<string>("ComponentType")
                .HasValue<CPU>("CPU")
                .HasValue<GPU>("GPU")
                .HasValue<Motherboard>("Motherboard")
                .HasValue<RAM>("RAM")
                .HasValue<Storage>("Storage");

            // Configure entity relationships
            modelBuilder.Entity<HardwareComponent>()
                .HasOne(c => c.Manufacturer)
                .WithMany(m => m.HardwareComponents)
                .HasForeignKey(c => c.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HardwareComponent>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.HardwareComponents)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Specification>()
                .HasOne(s => s.HardwareComponent)
                .WithMany(h => h.Specifications)
                .HasForeignKey(s => s.HardwareComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(p => p.HardwareComponent)
                .WithMany(h => h.PriceHistory)
                .HasForeignKey(p => p.HardwareComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.HardwareComponent)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HardwareComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScrapingLog>()
                .HasOne(l => l.ScrapingSource)
                .WithMany(s => s.ScrapingLogs)
                .HasForeignKey(l => l.ScrapingSourceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add indexes for performance
            modelBuilder.Entity<HardwareComponent>().HasIndex(c => c.Name);
            modelBuilder.Entity<HardwareComponent>().HasIndex(c => c.Model);
            modelBuilder.Entity<HardwareComponent>().HasIndex(c => c.ManufacturerId);
            modelBuilder.Entity<HardwareComponent>().HasIndex(c => c.CategoryId);
            modelBuilder.Entity<PriceHistory>().HasIndex(p => p.RecordedDate);
            modelBuilder.Entity<ScrapingLog>().HasIndex(l => l.StartTime);
        }

        // Automatically set created/modified dates
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }
}

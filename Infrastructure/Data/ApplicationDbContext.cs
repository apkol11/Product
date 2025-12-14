using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Colour> colours { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColour> ProductColours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship
            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColours)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.Colour)
                .WithMany()
                .HasForeignKey(pc => pc.ColourId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}


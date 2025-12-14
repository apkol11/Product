using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }  


        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<ProductModel> products { get; set; }
        public DbSet<Colour> colours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductType>().ToTable("ProductTypes");
            modelBuilder.Entity<ProductModel>().ToTable("Products");
            modelBuilder.Entity<Colour>().ToTable("Colours");

            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProdcutTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Colour)
                .WithMany()
                .HasForeignKey(p => p.ColourID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

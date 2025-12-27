using e_commerce_basic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Database
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Gallery> Galleries { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<SubCategory> SubCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config index entity
            modelBuilder.Entity<Product>().HasIndex(p => p.SubCategoryId);
            modelBuilder.Entity<SubCategory>().HasIndex(sc => sc.CategoryId);
            modelBuilder.Entity<Product>().HasIndex(p => p.CreatedAt);
            modelBuilder.Entity<Gallery>().HasIndex(g => g.ProductId);

            // config index dual entity
            modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.SubCategoryId, p.CreatedAt })
            .IsDescending(false, true);

            // config unique
            modelBuilder.Entity<Product>()
            .HasIndex(p => p.Code)
            .IsUnique();
            modelBuilder.Entity<Stock>()
            .HasIndex(s => s.ProductId)
            .IsUnique();

            // config one to one
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Stock)
            .WithOne(s => s.Product)
            .HasForeignKey<Stock>(s => s.ProductId);

            // config one to many
            modelBuilder.Entity<Gallery>()
            .HasOne(g => g.Product)
            .WithMany(p => p.Galleries)
            .HasForeignKey(g => g.ProductId);

            base.OnModelCreating(modelBuilder);

        }

    }
}
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public StoreDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                      .IsRequired();

                entity.Property(e => e.CompanyName)
                      .IsRequired();

                entity.Property(e => e.City)
                      .IsRequired();

                entity.Property(e => e.Country)
                      .IsRequired(false);

                entity.Property(e => e.Password)
                      .IsRequired(false);

                entity.HasMany(e => e.Orders)
                      .WithOne()
                      .HasForeignKey("MemberId");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName)
                      .IsRequired();

                entity.HasMany(e => e.Products)
                      .WithOne()
                      .HasForeignKey("CategoryId");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Freight)
                      .IsRequired();

                entity.Property(e => e.OrderDate)
                      .IsRequired();

                entity.Property(e => e.RequiredDate)
                      .IsRequired();

                entity.Property(e => e.ShippedDate)
                      .IsRequired();

                entity.HasOne(e => e.Member)
                      .WithMany(m => m.Orders)
                      .HasForeignKey(e => e.MemberId);

                entity.HasMany(e => e.OrderDetails)
                      .WithOne()
                      .HasForeignKey("OrderId");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.Property(e => e.UnitPrice)
                      .IsRequired();

                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.Property(e => e.Discount)
                      .IsRequired();

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.ProductName)
                      .IsRequired();

                entity.Property(e => e.UnitPrice)
                      .IsRequired();

                entity.Property(e => e.Weight)
                      .IsRequired();

                entity.Property(e => e.UnitsInStock)
                      .IsRequired();

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.CategoryId);

                entity.HasMany(e => e.OrderDetails)
                      .WithOne(od => od.Product)
                      .HasForeignKey(od => od.ProductId);
            });
        }
    }
}

using ChainMarket.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChainProduct>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockReceipt>()
                .HasOne(r => r.Supplier)
                .WithMany()
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockReceipt>()
                .HasOne(r => r.ChainProduct)
                .WithMany()
                .HasForeignKey(r => r.ChainProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(p => p.ChainProduct)
                .WithMany()
                .HasForeignKey(p => p.ChainProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChainOrder>()
                .HasOne(o => o.ChainProduct)
                .WithMany()
                .HasForeignKey(o => o.ChainProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ChainProduct> ChainProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockReceipt> StockReceipts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ChainOrder> ChainOrders { get; set; }
    }
}
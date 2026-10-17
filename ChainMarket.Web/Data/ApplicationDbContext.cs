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

            modelBuilder.Entity<StockReceipt>()
                .HasOne(r => r.Supplier)
                .WithMany()
                .HasForeignKey(r => r.SupplierId)
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
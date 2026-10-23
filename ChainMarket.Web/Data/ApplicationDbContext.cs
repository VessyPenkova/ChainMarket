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
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Ролкови вериги" },
                new Category { Id = 2, Name = "Транспортьорни вериги" },
                new Category { Id = 3, Name = "Подемни вериги" },
                new Category { Id = 4, Name = "Селскостопански вериги" },
                new Category { Id = 5, Name = "Неръждаеми и специални вериги" },
                new Category { Id = 6, Name = "Свързващи звена и аксесоари" }
            );
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerOrderItem>()
                .HasOne(i => i.CustomerOrder)
                .WithMany()
                .HasForeignKey(i => i.CustomerOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerOrderItem>()
                .HasOne(i => i.ChainProduct)
                .WithMany()
                .HasForeignKey(i => i.ChainProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChainStandard>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<ChainProduct>()
                .HasOne(p => p.ChainStandard)
                .WithMany()
                .HasForeignKey(p => p.ChainStandardId)
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<ChainProduct>()
                .HasOne(p => p.ChainNomenclature)
                .WithMany()
                .HasForeignKey(p => p.ChainNomenclatureId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChainNomenclature>()
                .HasOne(n => n.Category)
                .WithMany()
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChainNomenclature>()
                .HasOne(n => n.ChainStandard)
                .WithMany()
                .HasForeignKey(n => n.ChainStandardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChainNomenclature>()
                .HasIndex(n => new { n.CategoryId, n.Code })
                .IsUnique();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ChainStandard> ChainStandards { get; set; }
        public DbSet<ChainNomenclature> ChainNomenclatures { get; set; }
        public DbSet<ChainProduct> ChainProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockReceipt> StockReceipts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ChainOrder> ChainOrders { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }
    }
}
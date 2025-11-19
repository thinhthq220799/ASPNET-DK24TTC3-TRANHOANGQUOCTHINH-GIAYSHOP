using Microsoft.EntityFrameworkCore;
using SHOPGIAY.Models;

namespace SHOPGIAY.Data
{
    public class ShoeShopContext : DbContext
    {
        public ShoeShopContext(DbContextOptions<ShoeShopContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== CATEGORIES =====
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                      .HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                      .HasColumnName("category_name");

                entity.Property(e => e.Slug)
                      .HasColumnName("slug");

                entity.Property(e => e.Description)
                      .HasColumnName("description");
            });

            // ===== PRODUCTS =====
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                      .HasColumnName("product_id");

                entity.Property(e => e.CategoryId)
                      .HasColumnName("category_id");

                entity.Property(e => e.ProductName)
                      .HasColumnName("product_name");

                entity.Property(e => e.Sku)
                      .HasColumnName("sku");

                entity.Property(e => e.Size)
                      .HasColumnName("size");

                entity.Property(e => e.Color)
                      .HasColumnName("color");

                entity.Property(e => e.Price)
                      .HasColumnName("price");

                entity.Property(e => e.StockQuantity)
                      .HasColumnName("stock_quantity");

                entity.Property(e => e.Thumbnail)
                      .HasColumnName("thumbnail");

                entity.Property(e => e.Description)
                      .HasColumnName("description");

                entity.Property(e => e.IsActive)
                      .HasColumnName("is_active");

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== ORDERS =====
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id");

                entity.Property(e => e.CustomerName)
                      .HasColumnName("customer_name");

                entity.Property(e => e.CustomerPhone)
                      .HasColumnName("customer_phone");

                entity.Property(e => e.CustomerEmail)
                      .HasColumnName("customer_email");

                entity.Property(e => e.ShippingAddress)
                      .HasColumnName("shipping_address");

                entity.Property(e => e.OrderDate)
                      .HasColumnName("order_date");

                entity.Property(e => e.Status)
                      .HasColumnName("status");

                entity.Property(e => e.TotalAmount)
                      .HasColumnName("total_amount");

                entity.Property(e => e.Note)
                      .HasColumnName("note");
            });

            // ===== ORDER_DETAILS =====
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("order_details");

                entity.HasKey(e => e.OrderDetailId);

                entity.Property(e => e.OrderDetailId)
                      .HasColumnName("order_detail_id");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id");

                entity.Property(e => e.ProductId)
                      .HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                      .HasColumnName("quantity");

                entity.Property(e => e.UnitPrice)
                      .HasColumnName("unit_price");

                entity.Property(e => e.Discount)
                      .HasColumnName("discount");

                entity.Property(e => e.LineTotal)
                      .HasColumnName("line_total");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using NCTServices.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Infrastructure.Contexts
{
    public class NCTServicesDBWriteContext : NCTServicesSQLContext
    {
        public NCTServicesDBWriteContext(DbContextOptions<NCTServicesDBWriteContext> options) : base(options)
        {

        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categorie");
            });
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("OrderDetail");
                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order_OrderDetail");
                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_OrderDetail");

            });
            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("Order");
                entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Users_Orders");
            });
            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Product");
                entity.HasOne(d => d.Categories).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Categories_Products");
            });
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("User");
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess: true);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
    public class NCTServicesDBReadContext : NCTServicesSQLContext
    {
        public NCTServicesDBReadContext(DbContextOptions<NCTServicesDBReadContext> options) : base(options)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categorie");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("OrderDetail");
                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order_OrderDetail");
                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_OrderDetail");

            });
            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("Order");
                entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Users_Orders");
            });
            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Product");
                entity.HasOne(d => d.Categories).WithMany(p => p.Products)
                  .HasForeignKey(d => d.CategoryID)
                  .HasConstraintName("FK_Categories_Products");
            });
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("User");
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess: true);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

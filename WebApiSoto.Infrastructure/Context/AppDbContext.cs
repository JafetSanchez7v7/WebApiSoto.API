using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.Infrastructure.Context
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public virtual DbSet<Option> Options { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Personalization> Personalizations { get; set; }

        public virtual DbSet<PersonalizedProduct> PersonalizedProducts { get; set; }

        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==========================================
            // 1. TUS MODELOS ORIGINALES Y SUS RELACIONES
            // ==========================================

            modelBuilder.Entity<Users>(cfg => cfg.HasKey(src => src.UserId));
            modelBuilder.Entity<Categories>(cfg => cfg.HasKey(src => src.Id));
            modelBuilder.Entity<Customers>(cfg => cfg.HasKey(src => src.CustomerId));
            modelBuilder.Entity<Suppliers>(cfg => cfg.HasKey(src => src.SupplierId));

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(src => src.ProductId);

                // Relación: Una Categoría tiene muchos Productos
                entity.HasOne(d => d.Category)
                    .WithMany()
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                // Relación: Un Proveedor tiene muchos Productos
                entity.HasOne(d => d.Supplier)
                    .WithMany()
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            // ==========================================
            // 2. MODELOS NUEVOS (SCAFFOLDING GENERADO)
            // ==========================================

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B3F13CBCAC");
                entity.ToTable("Inventory");
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAB5080B3930");
                entity.ToTable("Invoice");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.PrintedDate).HasColumnType("datetime");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Sale).WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK__Invoice__SaleId__76969D2E");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__InvoiceD__3214EC075F758FC1");
                entity.Property(e => e.LineTotal).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK__InvoiceDe__Invoi__797309D9");
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasKey(e => e.OptionId).HasName("PK__Options__92C7A1FFF8A57986");
                entity.Property(e => e.Description).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.Measurement).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF865F7C3B");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.TimeDelivery).HasColumnType("datetime");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C0894F321");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Volume).HasMaxLength(50).IsUnicode(false);

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__628FA481");
            });

            modelBuilder.Entity<Personalization>(entity =>
            {
                entity.HasKey(e => e.PersonalizationId).HasName("PK__Personal__69D4B79E5E4541F3");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Option).WithMany(p => p.Personalizations)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK__Personali__Optio__6C190EBB");

                entity.HasOne(d => d.Personalized).WithMany(p => p.Personalizations)
                    .HasForeignKey(d => d.PersonalizedId)
                    .HasConstraintName("FK__Personali__Perso__6D0D32F4");
            });

            modelBuilder.Entity<PersonalizedProduct>(entity =>
            {
                entity.HasKey(e => e.PersonalizedId).HasName("PK__Personal__96BC6947BCFFAFB1");
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Order).WithMany(p => p.PersonalizedProducts)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Personali__Order__66603565");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BBEB2250F74");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<PurchaseDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC072660EA2C");
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.PurchaseId)
                    .HasConstraintName("FK__PurchaseD__Purch__5CD6CB2B");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C3FF8959E445");
                entity.Property(e => e.SaleDate).HasColumnType("datetime");
                entity.Property(e => e.SaleTotal).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__SaleDeta__3214EC0774E07D63");
                entity.Property(e => e.LineAmount).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.Volume).HasMaxLength(50).IsUnicode(false);

                entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK__SaleDetai__SaleI__72C60C4A");
            });

            
        }

        

    }
}

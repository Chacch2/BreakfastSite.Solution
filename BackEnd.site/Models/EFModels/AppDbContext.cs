using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BackEnd.site.Models.EFModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<AddOnCategory> AddOnCategories { get; set; }
        public virtual DbSet<AddOnOption> AddOnOptions { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<OrderAddOnDetail> OrderAddOnDetails { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductAddOnDetail> ProductAddOnDetails { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOnCategory>()
                .HasMany(e => e.AddOnOptions)
                .WithRequired(e => e.AddOnCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AddOnCategory>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.AddOnCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AddOnOption>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.AddOnOption)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.EncryptedPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .HasMany(e => e.OrderAddOnDetails)
                .WithOptional(e => e.OrderDetail)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Order)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ProductAddOnDetail>()
                .HasMany(e => e.OrderAddOnDetails)
                .WithOptional(e => e.ProductAddOnDetail)
                .HasForeignKey(e => e.ProductAddOnDetailsID);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}

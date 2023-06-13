using System;
using Market.Data;
using Market.Data.Users;
using Market.Data.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Market.Data.Stores;
using Market.Data.Couriers;
using Market.Data.Customers;
using Market.Data.Admins;
using Market.Data.Categories;
using Market.Data.Products;
using Market.Data.Orders;

namespace Market.Repository
{
	public class MarketContext : DbContext
	{
        
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CourierApplication> CourierApplications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }



        public MarketContext()
        {

        }
       
		public MarketContext(DbContextOptions<MarketContext> options) : base(options) {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=MarketDb; User Id=sa; Password=Strong.Pwd-123");
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(bc => new { bc.OrderId, bc.ProductId });
            modelBuilder.Entity<OrderProduct>()
                .HasOne(bc => bc.Order)
                .WithMany(b => b.OrderProducts)
                .HasForeignKey(bc => bc.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(c => c.OrderProducts)
                .HasForeignKey(bc => bc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Modify the cascade action here

            // Configure the relationship between Order and Courier
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Courier)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.Restrict); // Modify the cascade action here

            // Configure the relationship between Order and Store
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

           

       
                

        }

    }
}


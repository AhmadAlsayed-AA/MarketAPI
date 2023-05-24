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


        public MarketContext()
        {

        }
       
		public MarketContext(DbContextOptions<MarketContext> options) : base(options) {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=MarketDb; User Id=sa; Password=Strong.Pwd-123");
            
        }

    }
}


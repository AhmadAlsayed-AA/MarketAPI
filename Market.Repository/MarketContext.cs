using System;
using Market.Data;
using Market.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace Market.Repository
{
	public class MarketContext : DbContext
	{
		public MarketContext()
		{
		}
		public MarketContext(DbContextOptions<MarketContext> options) : base(options) {
        }
		public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;database=MarketDB;User=sa;Password=Strong.Pwd-123;");
            }
        }

    }
}


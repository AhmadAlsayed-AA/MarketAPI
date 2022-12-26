using System;
using Market.Data.Addresses;
using Market.Data.Users;

namespace Market.Data.Customers
{
	public class Customer
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Address> Addresses { get; set; }
        //public ICollection<Order>? Orders { get; set; }
    }
}


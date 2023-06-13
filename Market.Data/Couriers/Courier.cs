using System;
using Market.Data.Orders;
using Market.Data.Users;

namespace Market.Data.Couriers
{
	public class Courier
	{
		public int Id { get; set; }

		public string PersonalPhoto { get; set; }

		public bool IsActive { get; set; }

        public int UserId { get; set; }

		public User User { get; set; }

		public ICollection<Order> Orders { get; set; }
	}
}


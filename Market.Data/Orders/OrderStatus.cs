using System;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Orders
{
	public class OrderStatus
	{
		public int Id { get; set; }

		public DeliveryStatus Status { get; set; }

		public DateTime UpdatedAt { get; set; }

		public int OrderId { get; set; }

		public Order Order { get; set; }
	}
}


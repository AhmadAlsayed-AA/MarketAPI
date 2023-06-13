using System;
using Market.Data.Addresses;
using Market.Data.Couriers;
using Market.Data.Customers;
using Market.Data.Stores;

namespace Market.Data.Orders
{
	public class Order
	{
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentDetails { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int CourierId { get; set; }
        public Courier Courier { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }

        public ICollection<OrderStatus> OrderStatuses { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}


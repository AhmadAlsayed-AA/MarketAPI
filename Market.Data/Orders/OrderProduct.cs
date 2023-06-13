using System;
using System.ComponentModel.DataAnnotations.Schema;
using Market.Data.Products;

namespace Market.Data.Orders
{
	public class OrderProduct
	{
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}


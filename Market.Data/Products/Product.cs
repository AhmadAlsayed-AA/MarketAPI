﻿using System;
using Market.Data.Categories;
using Market.Data.Orders;
using Market.Data.Stores;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Products
{
	public class Product
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public double? Weight { get; set; }

        public WeightUnit WeightUnit { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}


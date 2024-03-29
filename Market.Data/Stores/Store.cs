﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Market.Data.Addresses;
using System.Text.Json.Serialization;
using Market.Data.Users;
using Microsoft.AspNetCore.Http;
using Market.Data.Products;
using Market.Data.Categories;
using Market.Data.Orders;

namespace Market.Data.Stores
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}


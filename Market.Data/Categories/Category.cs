using System;
using Market.Data.Products;
using Market.Data.Stores;

namespace Market.Data.Categories
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Store> Stores { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}


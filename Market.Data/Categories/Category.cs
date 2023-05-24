using System;
using Market.Data.Products;

namespace Market.Data.Categories
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        // One-to-Many Relationship: One category can have multiple products
        public ICollection<Product> Products { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market.Data.Products;
using Market.Repository;
using Microsoft.EntityFrameworkCore;

namespace Market.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<List<Product>> GetProductsByStoreId(int storeId);
        Task Create(Product product);
        Task Update(int id, Product updatedProduct);
        Task Delete(int id);
    }

    public class ProductService : IProductService, IDisposable
    {
        private readonly MarketContext _context;

        public ProductService(MarketContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Product>> GetProductsByStoreId(int storeId)
        {
            return await _context.Products.Where(p => p.StoreId == storeId).ToListAsync();
        }

        public async Task Create(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                throw new ArgumentNullException(nameof(updatedProduct));
            }

            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            // Update other properties as needed

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

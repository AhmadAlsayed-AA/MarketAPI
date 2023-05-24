using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Categories;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;

namespace Market.Services.ProductServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int id);
        Task Create(CategoryRequest category);
        Task Update(int id, Category updatedCategory);
        Task Delete(int id);
    }

    public class CategoryService : ICategoryService, IDisposable
    {
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public CategoryService(MarketContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task Create(CategoryRequest category)
        {
            var newCategory = _mapper.Map<Category>(category);
            newCategory.CreatedAt = DateTime.Now;
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Category updatedCategory)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            // Update properties of the category
            category.Name = updatedCategory.Name;
            // Update other properties as needed

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PetMingle.Entities;

namespace PetMingle.Data.Repositories.Admin
{
    public class CategoryRepository : IDisposable
    {
        private readonly DataContext _ctx;

        public CategoryRepository(DataContext ctx)
        {
            _ctx = ctx;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _ctx.Categories.FindAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _ctx.Categories.Include(a => a.ProductCategories).ToListAsync();
            return categories;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            _ctx.Categories.Remove(category);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Category> UpdateCategoryAsync(Category updatedCategory)
        {
            var category = await GetCategoryByIdAsync(updatedCategory.ID);
            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;
            await _ctx.SaveChangesAsync();
            return category;
        }
    }
}

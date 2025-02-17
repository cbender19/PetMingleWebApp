using Microsoft.EntityFrameworkCore;
using PetMingle.Entities;

namespace PetMingle.Data.Repositories.Admin
{
    public class ProductRepository : IDisposable
    {
        private readonly DataContext _ctx;

        public ProductRepository(DataContext ctx)
        {
            _ctx = ctx;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {

            return await _ctx.Products.FirstOrDefaultAsync(p => p.ID == id);

        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _ctx.Products.Include(c => c.Company).Include(c => c.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
            return products;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            var product = await GetProductByIdAsync(updatedProduct.ID);
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.CompanyId = updatedProduct.CompanyId;
            product.Price = updatedProduct.Price;
            product.StockQuantity = updatedProduct.StockQuantity;
            product.ProductCoverUrl = updatedProduct.ProductCoverUrl;
            await _ctx.SaveChangesAsync();
            return product;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EshopDbContext _context;
        public ProductRepository(EshopDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Product>?> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product?> AddAsync(Product product)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
            _context.Products.Add(newProduct);
            var result = await _context.SaveChangesAsync();
            return result >= 0 ? newProduct : null;
        }
        public async Task<Product?> UpdateAsync(Product product)
        {
            var updatedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (updatedProduct != null)
            {
                updatedProduct.Name = product.Name;
                updatedProduct.Price = product.Price;
                updatedProduct.Description = product.Description;
                var result = await _context.SaveChangesAsync();
                return result >= 0 ? updatedProduct : null;
            }
            return null;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var removedProduct = await _context.Products.FindAsync(id);
            if (removedProduct != null)
            {
                _context.Products.Remove(removedProduct);
                var result = await _context.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }
    }
}

using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<Product>?> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}

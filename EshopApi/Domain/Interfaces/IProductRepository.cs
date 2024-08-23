using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<Product>?> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByNameAsync(string name);
        Task<ICollection<Product>?> GetByPageAsync(int pageNumber, int pageSize);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}

using EshopApi.Application.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ICollection<ProductDTO>?> GetAllProductAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO?> AddNewProductAsync(ProductNewDTO product);
        Task<ProductDTO?> UpdateProductAsync(ProductDTO product);
        Task<bool> DeleteProductAsync(int id);
    }
}

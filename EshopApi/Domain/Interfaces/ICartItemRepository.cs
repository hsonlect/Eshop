using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task<ICollection<CartItem>?> GetAllAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem?> AddAsync(CartItem cartItem);
        Task<CartItem?> UpdateAsync(CartItem cartItem);
        Task<bool> DeleteAsync(int id);
    }
}

using EshopApi.Domain.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface ICartItemService
    {
        Task<ICollection<CartItemDTO>?> GetAllCartItemAsync();
        Task<CartItemDTO?> GetCartItemByIdAsync(int id);
        Task<CartItemDTO?> AddNewCartItemAsync(CartItemNewDTO cartItem);
        Task<CartItemDTO?> UpdateCartItemAsync(CartItemDTO cartItem);
        Task<bool> DeleteCartItemAsync(int id);
    }
}

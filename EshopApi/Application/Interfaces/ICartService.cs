using EshopApi.Application.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface ICartService
    {
        Task<ICollection<CartItemDTO>?> GetAllCartItemByUserIdAsync(int userId);
        Task<bool> AddNewItemToCartAsync(int userId, int productId);
        Task<bool> RemoveItemFromCartAsync(int userId, int productId);
    }
}

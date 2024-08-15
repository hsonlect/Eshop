using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ICollection<CartItemDTO>?> GetAllCartItemByUserIdAsync(int userId)
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            if (cartItems == null)
            {
                return null;
            }
            return cartItems.Where(ci => ci.UserId == userId).Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                UserId = ci.UserId,
                ProductId = ci.ProductId
            }).ToList();
        }
        public async Task<bool> AddNewItemToCartAsync(int userId, int productId)
        {
            var newCartItem = await _cartItemRepository.AddAsync(new CartItem()
            {
                UserId = userId,
                ProductId = productId
            });
            return newCartItem != null;
        }

        public async Task<bool> RemoveItemFromCartAsync(int userId, int productId)
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            if (cartItems == null)
            {
                return false;
            }
            var removeItem = cartItems.Where(ci => ci.UserId == userId && ci.ProductId == productId)
            .Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                UserId = ci.UserId,
                ProductId = ci.ProductId
            }).FirstOrDefault();
            if (removeItem != null)
            {
                return await _cartItemRepository.DeleteAsync(removeItem.Id);
            }
            return false;
        }
    }
}

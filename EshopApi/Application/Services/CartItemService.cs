using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ICollection<CartItemDTO>?> GetAllCartItemAsync()
        {
            var cartItems = await _cartItemRepository.GetAllAsync();
            if (cartItems == null)
            {
                return null;
            }
            return cartItems.Select(ci => new CartItemDTO
            {
                Id = ci.Id,
                UserId = ci.UserId,
                ProductId = ci.ProductId
            }).ToList();
        }

        public async Task<CartItemDTO?> GetCartItemByIdAsync(int id)
        {
            var cartItems = await _cartItemRepository.GetByIdAsync(id);
            if (cartItems == null)
            {
                return null;
            }
            return new CartItemDTO
            {
                Id = cartItems.Id,
                UserId = cartItems.UserId,
                ProductId = cartItems.ProductId
            };
        }

        public async Task<CartItemDTO?> AddNewCartItemAsync(CartItemNewDTO cartItem)
        {
            var newCartItem = await _cartItemRepository.AddAsync(new CartItem()
            {
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId
            });
            if (newCartItem == null)
            {
                return null;
            }
            return new CartItemDTO
            {
                Id = newCartItem.Id,
                UserId = newCartItem.UserId,
                ProductId = newCartItem.ProductId
            };
        }

        public async Task<CartItemDTO?> UpdateCartItemAsync(CartItemDTO cartItem)
        {
            var oldCartItem = await _cartItemRepository.GetByIdAsync(cartItem.Id);
            if (oldCartItem == null)
            {
                return null;
            }
            oldCartItem.UserId = cartItem.UserId;
            oldCartItem.ProductId = cartItem.ProductId;
            var newCartItem = await _cartItemRepository.UpdateAsync(oldCartItem);
            if (newCartItem == null)
            {
                return null;
            }
            return new CartItemDTO
            {
                Id = newCartItem.Id,
                UserId = newCartItem.UserId,
                ProductId = newCartItem.ProductId
            };
        }

        public async Task<bool> DeleteCartItemAsync(int id)
        {
            return await _cartItemRepository.DeleteAsync(id);
        }
    }
}

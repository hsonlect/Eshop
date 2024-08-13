using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly EshopDbContext _context;
        public CartItemRepository(EshopDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<CartItem>?> GetAllAsync()
        {
            return await _context.CartItems.ToListAsync();
        }
        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == id);
        }
        public async Task<CartItem?> AddAsync(CartItem cartItem)
        {
            var newCartItem = new CartItem()
            {
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId
            };
            _context.CartItems.Add(newCartItem);
            var result = await _context.SaveChangesAsync();
            return result >= 0 ? newCartItem : null;
        }
        public async Task<CartItem?> UpdateAsync(CartItem cartItem)
        {
            var updatedCartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItem.Id);
            if (updatedCartItem != null)
            {
                updatedCartItem.UserId = cartItem.UserId;
                updatedCartItem.ProductId = cartItem.ProductId;
                var result = await _context.SaveChangesAsync();
                return result >= 0 ? updatedCartItem : null;
            }
            return null;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var removedCartItem = await _context.CartItems.FindAsync(id);
            if (removedCartItem != null)
            {
                _context.CartItems.Remove(removedCartItem);
                var result = await _context.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }
    }
}

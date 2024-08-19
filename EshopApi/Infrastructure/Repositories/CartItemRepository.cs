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
            return await _context.CartItems.FindAsync(id);
        }

        public async Task<ICollection<CartItem>?> GetByUserIdAsync(int userId)
        {
            return await _context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
        }

        public async Task<CartItem?> GetByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await _context.CartItems.Where(ci => ci.UserId == userId && ci.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<CartItem?> AddAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            var result = await _context.SaveChangesAsync();
            return (result > 0) ? cartItem : null;
        }

        public async Task<CartItem?> UpdateAsync(CartItem cartItem)
        {
            var updatedCartItem = await _context.CartItems.FindAsync(cartItem.Id);
            if (updatedCartItem == null)
            {
                return null;
            }
            updatedCartItem.UserId = cartItem.UserId;
            updatedCartItem.ProductId = cartItem.ProductId;
            var result = await _context.SaveChangesAsync();
            return (result > 0) ? updatedCartItem : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var removedCartItem = await _context.CartItems.FindAsync(id);
            if (removedCartItem == null)
            {
                return false;
            }
            _context.CartItems.Remove(removedCartItem);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

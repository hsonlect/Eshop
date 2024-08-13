using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EshopDbContext _context;
        public UserRepository(EshopDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<User>?> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> AddAsync(User user)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existUser != null)
            {
                return null;
            }
            var newUser = new User()
            {
                Username = user.Username,
                Role = user.Role,
                Password = user.Password
            };
            _context.Users.Add(newUser);
            var result = await _context.SaveChangesAsync();
            return result >= 0 ? newUser : null;
        }
        public async Task<User?> UpdateAsync(User user)
        {
            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (updatedUser != null)
            {
                updatedUser.Username = user.Username;
                updatedUser.Role = user.Role;
                updatedUser.Password = user.Password;
                var result = await _context.SaveChangesAsync();
                return result >= 0 ? updatedUser : null;
            }
            return null;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var removedUser = await _context.Users.FindAsync(id);
            if (removedUser != null)
            {
                _context.Users.Remove(removedUser);
                var result = await _context.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }
    }
}

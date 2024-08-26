using EshopApi.Domain.Entities;

namespace EshopApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>?> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}

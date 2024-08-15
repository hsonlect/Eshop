using EshopApi.Domain.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>?> GetAllUserAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO?> AddNewUserAsync(UserNewDTO user);
        Task<UserDTO?> UpdateUserAsync(UserDTO user);
        Task<UserDTO?> UpdateUserPasswordAsync(int id, string password);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDTO?> AuthenticateUserAsync(string username, string password);
    }
}

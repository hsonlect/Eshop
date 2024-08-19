using EshopApi.Domain.DTOs;

namespace EshopApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>?> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO?> GetUserByUsernameAndPasswordAsync(string username, string password);
        Task<UserDTO?> AddNewUserAsync(UserNewDTO userNewDto);
        Task<UserDTO?> UpdateUserAsync(UserDTO userDto);
        Task<UserDTO?> UpdateUserPasswordAsync(int id, string password);
        Task<bool> DeleteUserAsync(int id);
    }
}

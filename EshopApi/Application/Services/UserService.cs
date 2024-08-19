using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private UserDTO ToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICollection<UserDTO>?> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users?.Select(ToUserDTO).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return (user != null) ? ToUserDTO(user) : null;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return (user != null) ? ToUserDTO(user) : null;
        }

        public async Task<UserDTO?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if ((user == null) || (user.Password != password))
            {
                return null;
            }
            return ToUserDTO(user);
        }

        public async Task<UserDTO?> AddNewUserAsync(UserNewDTO userNewDto)
        {
            var addedUser = new User
            {
                Username = userNewDto.Username,
                Role = userNewDto.Role,
                Password = userNewDto.Password
            };
            addedUser = await _userRepository.AddAsync(addedUser);
            return (addedUser != null) ? ToUserDTO(addedUser) : null;
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO userDto)
        {
            var updatedUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Username = userDto.Username;
            updatedUser.Role = userDto.Role;
            updatedUser = await _userRepository.UpdateAsync(updatedUser);
            return (updatedUser != null) ? ToUserDTO(updatedUser) : null;
        }

        public async Task<UserDTO?> UpdateUserPasswordAsync(int id, string password)
        {
            var updatedUser = await _userRepository.GetByIdAsync(id);
            if (updatedUser == null)
            {
                return null;
            }
            updatedUser.Password = password;
            updatedUser = await _userRepository.UpdateAsync(updatedUser);
            return (updatedUser != null) ? ToUserDTO(updatedUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}

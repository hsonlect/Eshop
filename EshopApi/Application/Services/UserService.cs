using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICollection<UserDTO>?> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users != null)
            {
                return users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role
                }).ToList();
            }
            return null;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role
                };
            }
            return null;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<UserDTO?> AddNewUserAsync(UserNewDTO user)
        {
            var newUser = await _userRepository.AddAsync(new User()
            {
                Username = user.Username,
                Role = user.Role,
                Password = user.Password
            });
            if (newUser == null)
            {
                return null;
            }
            return new UserDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Role = newUser.Role
            };
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO user)
        {
            var oldUser = await _userRepository.GetByIdAsync(user.Id);
            if (oldUser == null)
            {
                return null;
            }
            oldUser.Username = user.Username;
            oldUser.Role = user.Role;
            var newUser = await _userRepository.UpdateAsync(oldUser);
            if (newUser == null)
            {
                return null;
            }
            return new UserDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Role = newUser.Role
            };
        }

        public async Task<UserDTO?> UpdateUserPasswordAsync(int id, string password)
        {
            var oldUser = await _userRepository.GetByIdAsync(id);
            if (oldUser == null)
            {
                return null;
            }
            oldUser.Password = password;
            var newUser = await _userRepository.UpdateAsync(oldUser);
            if (newUser == null)
            {
                return null;
            }
            return new UserDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Role = newUser.Role
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDTO?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if ((user == null) || (user.Password != password))
            {
                return null;
            }
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}

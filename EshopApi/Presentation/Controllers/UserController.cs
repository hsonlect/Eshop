using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EshopApi.Domain.DTOs;
using EshopApi.Application.Interfaces;
using EshopApi.Presentation.Utils;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(new ResponseMessage<ICollection<UserDTO>>()
            {
                Status = true,
                Message = "Get all users successfully",
                Data = users
            });
        }

        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Get user by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Get user by id successfully",
                    Data = user
                });
            }
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(UserNewDTO user)
        {
            var newUser = await _userService.AddNewUserAsync(user);
            if (newUser == null)
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Add new user failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Add new user successfully!!!",
                    Data = newUser
                });
            }
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Update user failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Update user successfully!!!",
                    Data = updatedUser
                });
            }
        }

        [HttpPut("updateUser/{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO user)
        {
            if ((id == 0) || (user.Id == 0) || (id != user.Id))
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Invalid user id"
                });
            }
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Update user by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Update user by id successfully!!!",
                    Data = updatedUser
                });
            }
        }

        [HttpPut("updateUserPassword/{id}")]
        public async Task<IActionResult> UpdateUserPassword(int id, string password)
        {
            var updatedUser = await _userService.UpdateUserPasswordAsync(id, password);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Update user password by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Update user password by id successfully!!!",
                    Data = updatedUser
                });
            }
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == false)
            {
                return BadRequest(new ResponseMessage<UserDTO>()
                {
                    Status = false,
                    Message = "Delete user by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<UserDTO>()
                {
                    Status = true,
                    Message = "Delete user by id successfully!!!",
                });
            }
        }
    }
}

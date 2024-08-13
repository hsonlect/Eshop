using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using EshopApi.Application.DTOs;
using EshopApi.Application.Interfaces;
using EshopApi.Presentation.Utils;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if ((HttpContext.User.Identity != null) && HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest(new ResponseMessage<string>
                {
                    Status = false,
                    Message = "User was already logged in"
                });
            }
            var user = await _userService.AuthenticateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new ResponseMessage<string>
                {
                    Status = false,
                    Message = "Invalid username or password"
                });
            }
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, user.Role),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
            return Ok(new ResponseMessage<LoginResponse>
            {
                Status = true,
                Message = "Logged in successfully",
                Data = new LoginResponse
                {
                    Username = user.Username,
                    Role = user.Role
                }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if ((HttpContext.User.Identity == null) || !HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest(new ResponseMessage<string>
                {
                    Status = false,
                    Message = "User was already logged out"
                });
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new ResponseMessage<string>()
            {
                Status = true,
                Message = "Logged out successfully"
            });
        }
    }
}

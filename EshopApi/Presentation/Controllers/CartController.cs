using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EshopApi.Domain.DTOs;
using EshopApi.Application.Interfaces;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IUserService _userservice;
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IUserService userservice, ICartItemService cartItemService, IProductService productService)
        {
            _userservice = userservice;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _productService = productService;
        }

        [HttpGet("getCart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetCart()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var cartItems = await _cartService.GetAllCartItemByUserIdAsync(user.Id);
            var products = new List<ProductDTO>();
            if (cartItems != null)
            {
                foreach (var cartItem in cartItems)
                {
                    var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }
            }
            return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
            {
                Status = true,
                Message = "Get all cart items successfully",
                Data = products
            });
        }

        [HttpGet("getCart/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cartItems = await _cartService.GetAllCartItemByUserIdAsync(userId);
            return Ok(new ResponseWrapperDTO<ICollection<CartItemDTO>>()
            {
                Status = true,
                Message = "Get all cart items by user id successfully",
                Data = cartItems
            });
        }

        [HttpPost("addToCart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddToCart(AddToCartReqDTO requestDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var addedItem = await _cartItemService.AddNewCartItemAsync(new CartItemNewDTO
            {
                UserId = user.Id,
                ProductId = requestDto.Id
            });
            if (addedItem == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Add item to cart failed",
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Add item to cart successfully",
            });
        }

        [HttpGet("removeFromCart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return NotFound(new ResponseWrapperDTO<ICollection<string>>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var user = await _userservice.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new ResponseWrapperDTO<ICollection<string>>()
                {
                    Status = false,
                    Message = "Invalid username",
                });
            }
            var result = await _cartService.RemoveItemFromCartAsync(user.Id, productId);
            if (result == false)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Remove item from cart failed",
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Remove item from cart successfully",
            });
        }
    }
}

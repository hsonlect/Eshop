using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EshopApi.Application.DTOs;
using EshopApi.Application.Interfaces;
using EshopApi.Presentation.Utils;

namespace EshopApi.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize(Roles = "admin")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet("getCartItem")]
        public async Task<IActionResult> GetCartItem()
        {
            var cartItems = await _cartItemService.GetAllCartItemAsync();
            return Ok(new ResponseMessage<ICollection<CartItemDTO>>()
            {
                Status = true,
                Message = "Get all cart items successfully",
                Data = cartItems
            });
        }

        [HttpGet("getCartItem/{id}")]
        public async Task<IActionResult> GetCartItem(int id)
        {
            var cartItem = await _cartItemService.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Get cart item by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<CartItemDTO>()
                {
                    Status = true,
                    Message = "Get cart item by id successfully",
                    Data = cartItem
                });
            }
        }

        [HttpPost("addCartItem")]
        public async Task<IActionResult> AddCartItem(CartItemNewDTO cartItem)
        {
            var newCartItem = await _cartItemService.AddNewCartItemAsync(cartItem);
            if (newCartItem == null)
            {
                return BadRequest(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Add new cart item failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<CartItemDTO>()
                {
                    Status = true,
                    Message = "Add new cart item successfully!!!",
                    Data = newCartItem
                });
            }
        }

        [HttpPut("updateCartItem")]
        public async Task<IActionResult> UpdateCartItem(CartItemDTO cartItem)
        {
            var updatedCartItem = await _cartItemService.UpdateCartItemAsync(cartItem);
            if (updatedCartItem == null)
            {
                return BadRequest(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Update cart item failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<CartItemDTO>()
                {
                    Status = true,
                    Message = "Update cart item successfully!!!",
                    Data = updatedCartItem
                });
            }
        }

        [HttpPut("updateCartItem/{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateCartItem(int id, CartItemDTO cartItem)
        {
            if ((id == 0) || (cartItem.Id == 0) || (id != cartItem.Id))
            {
                return BadRequest(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Invalid cart item id"
                });
            }
            var updatedCartItem = await _cartItemService.UpdateCartItemAsync(cartItem);
            if (updatedCartItem == null)
            {
                return BadRequest(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Update cart item by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<CartItemDTO>()
                {
                    Status = true,
                    Message = "Update cart item by id successfully!!!",
                    Data = updatedCartItem
                });
            }
        }

        [HttpDelete("deleteCartItem/{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var result = await _cartItemService.DeleteCartItemAsync(id);
            if (result == false)
            {
                return BadRequest(new ResponseMessage<CartItemDTO>()
                {
                    Status = false,
                    Message = "Delete cart item by id failed"
                });
            }
            else
            {
                return Ok(new ResponseMessage<CartItemDTO>()
                {
                    Status = true,
                    Message = "Delete cart item by id successfully!!!",
                });
            }
        }
    }
}

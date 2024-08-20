using Asp.Versioning;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // [HttpGet("getProduct")]
        // public async Task<IActionResult> GetProduct()
        // {
        //     var products = await _productService.GetAllProductsAsync();
        //     return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
        //     {
        //         Status = true,
        //         Message = "Get all products successfully!!!",
        //         Data = products
        //     });
        // }

        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct([FromQuery] int? pageId = null, [FromQuery] int? pageSize = null)
        {
            var products = await _productService.GetAllProductsAsync() ?? [];
            if (pageId.HasValue && pageSize.HasValue)
            {
                var pagedProducts = products
                    .Skip((pageId.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
                return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
                {
                    Status = true,
                    Message = "Get product by page id successfully!!!",
                    Data = pagedProducts
                });
            }
            return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
            {
                Status = true,
                Message = "Get all products successfully!!!",
                Data = products
            });
        }

        [HttpGet("getProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Get product by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<ProductDTO>()
            {
                Status = true,
                Message = "Get product by id successfully!!!",
                Data = product
            });
        }

        [HttpGet("getProduct/page/{pageId}")]
        public async Task<IActionResult> GetProductByPage(int pageId)
        {
            const int pageSize = 5;
            var products = await _productService.GetAllProductsAsync() ?? [];
            var pagedProducts = products
                .Skip((pageId - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
            {
                Status = true,
                Message = "Get product by page id successfully!!!",
                Data = pagedProducts
            });
        }

        [HttpPost("addProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct(ProductNewDTO productNewDto)
        {
            var addedProduct = await _productService.AddNewProductAsync(productNewDto);
            if (addedProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Add new product failed"
                });
            }
            return Ok(new ResponseWrapperDTO<ProductDTO>()
            {
                Status = true,
                Message = "Add new product successfully!!!",
                Data = addedProduct
            });
        }

        [HttpPut("updateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(ProductDTO productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(productDto);
            if (updatedProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update product failed"
                });
            }
            return Ok(new ResponseWrapperDTO<ProductDTO>()
            {
                Status = true,
                Message = "Update product successfully!!!",
                Data = updatedProduct
            });
        }

        [HttpPut("updateProduct/{id}")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            if ((id == 0) || (productDto.Id == 0) || (id != productDto.Id))
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid product id"
                });
            }
            var updatedProduct = await _productService.UpdateProductAsync(productDto);
            if (updatedProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update product by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<ProductDTO>()
            {
                Status = true,
                Message = "Update product by id successfully!!!",
                Data = updatedProduct
            });
        }

        [HttpDelete("deleteProduct/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result == false)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Delete product by id failed"
                });
            }
            return Ok(new ResponseWrapperDTO<string>()
            {
                Status = true,
                Message = "Delete product by id successfully!!!",
            });
        }
    }
}

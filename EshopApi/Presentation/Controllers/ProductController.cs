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

        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(new ResponseWrapperDTO<ICollection<ProductDTO>>()
            {
                Status = true,
                Message = "Get all products successfully",
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
            else
            {
                return Ok(new ResponseWrapperDTO<ProductDTO>()
                {
                    Status = true,
                    Message = "Get product by id successfully",
                    Data = product
                });
            }
        }

        [HttpPost("addProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct(ProductNewDTO product)
        {
            var newProduct = await _productService.AddNewProductAsync(product);
            if (newProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Add new product failed"
                });
            }
            else
            {
                return Ok(new ResponseWrapperDTO<ProductDTO>()
                {
                    Status = true,
                    Message = "Add new product successfully!!!",
                    Data = newProduct
                });
            }
        }

        [HttpPut("updateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(ProductDTO product)
        {
            var updatedProduct = await _productService.UpdateProductAsync(product);
            if (updatedProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update product failed"
                });
            }
            else
            {
                return Ok(new ResponseWrapperDTO<ProductDTO>()
                {
                    Status = true,
                    Message = "Update product successfully!!!",
                    Data = updatedProduct
                });
            }
        }

        [HttpPut("updateProduct/{id}")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO product)
        {
            if ((id == 0) || (product.Id == 0) || (id != product.Id))
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Invalid product id"
                });
            }
            var updatedProduct = await _productService.UpdateProductAsync(product);
            if (updatedProduct == null)
            {
                return BadRequest(new ResponseWrapperDTO<string>()
                {
                    Status = false,
                    Message = "Update product by id failed"
                });
            }
            else
            {
                return Ok(new ResponseWrapperDTO<ProductDTO>()
                {
                    Status = true,
                    Message = "Update product by id successfully!!!",
                    Data = updatedProduct
                });
            }
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
            else
            {
                return Ok(new ResponseWrapperDTO<string>()
                {
                    Status = true,
                    Message = "Delete product by id successfully!!!",
                });
            }
        }
    }
}

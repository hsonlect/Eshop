using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ICollection<ProductDTO>?> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null)
            {
                return null;
            }
            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description
            }).ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        public async Task<ProductDTO?> AddNewProductAsync(ProductNewDTO product)
        {
            var newProduct = await _productRepository.AddAsync(new Product()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            });
            if (newProduct == null)
            {
                return null;
            }
            return new ProductDTO
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price,
                Description = newProduct.Description
            };
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO product)
        {
            var oldProduct = await _productRepository.GetByIdAsync(product.Id);
            if (oldProduct == null)
            {
                return null;
            }
            oldProduct.Name = product.Name;
            oldProduct.Price = product.Price;
            oldProduct.Description = product.Description;
            var newProduct = await _productRepository.UpdateAsync(oldProduct);
            if (newProduct == null)
            {
                return null;
            }
            return new ProductDTO
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price,
                Description = newProduct.Description
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}

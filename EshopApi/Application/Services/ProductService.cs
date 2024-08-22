using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ICollection<ProductDTO>?> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products?.Select(ToProductDTO).ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ProductDTO?> GetProductByNameAsync(string name)
        {
            var product = await _productRepository.GetByNameAsync(name);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ICollection<ProductDTO>?> GetProductByPageAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetByPageAsync(pageNumber, pageSize);
            return products?.Select(ToProductDTO).ToList();
        }

        public async Task<ProductDTO?> AddNewProductAsync(ProductNewDTO productNewDto)
        {
            var addedProduct = new Product
            {
                Name = productNewDto.Name,
                Price = productNewDto.Price,
                Description = productNewDto.Description
            };
            addedProduct = await _productRepository.AddAsync(addedProduct);
            return (addedProduct != null) ? ToProductDTO(addedProduct) : null;
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO productDto)
        {
            var updatedProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (updatedProduct == null)
            {
                return null;
            }
            updatedProduct.Name = productDto.Name;
            updatedProduct.Price = productDto.Price;
            updatedProduct.Description = productDto.Description;
            updatedProduct = await _productRepository.UpdateAsync(updatedProduct);
            return (updatedProduct != null) ? ToProductDTO(updatedProduct) : null;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}

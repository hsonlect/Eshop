using EshopApi.Domain.Entities;
using EshopApi.Domain.DTOs;
using EshopApi.Domain.Interfaces;
using EshopApi.Application.Interfaces;

namespace EshopApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
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

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<ProductDTO>?> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return products?.Select(ToProductDTO).ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ProductDTO?> GetProductByNameAsync(string name)
        {
            var product = await _unitOfWork.ProductRepository.GetByNameAsync(name);
            return (product != null) ? ToProductDTO(product) : null;
        }

        public async Task<ICollection<ProductDTO>?> GetProductByPageAsync(int pageNumber, int pageSize)
        {
            var products = await _unitOfWork.ProductRepository.GetByPageAsync(pageNumber, pageSize);
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
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    addedProduct = await _unitOfWork.ProductRepository.AddAsync(addedProduct);
                }, cts.Token);
                return ToProductDTO(addedProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO productDto)
        {
            var updatedProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productDto.Id);
            if (updatedProduct == null)
            {
                return null;
            }
            updatedProduct.Name = productDto.Name;
            updatedProduct.Price = productDto.Price;
            updatedProduct.Description = productDto.Description;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    updatedProduct = await _unitOfWork.ProductRepository.UpdateAsync(updatedProduct);
                }, cts.Token);
                return ToProductDTO(updatedProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.ProductRepository.DeleteAsync(id);
                }, cts.Token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

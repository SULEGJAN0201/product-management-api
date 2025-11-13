using ProductManagment.Core.Services;
using ProductManagment.Domain.Constants;
using ProductManagment.Domain.DTOs;
using ProductManagment.Domain.Entities;
using ProductManagment.Infrastructure.Data;

namespace ProductManagment.Services.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductReadDto> GetProductAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product Id must be greater than 0", nameof(productId));

            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException(ValidationMessages.ProductNotFound);

            return MapToReadDto(product);
        }

        public async Task<List<ProductReadDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();
            return products.Select(MapToReadDto).ToList();
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name.Trim(),
                Price = createDto.Price,
            };

            int productId = await _unitOfWork.ProductRepository.CreateProductAsync(product);

            if (productId <= 0)
                throw new InvalidOperationException("Failed to create product");

            // Fetch and return the created product
            var createdProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            if (createdProduct == null)
                throw new InvalidOperationException("Failed to retrieve created product");

            return MapToReadDto(createdProduct);
        }

        public async Task<ProductReadDto> UpdateProductAsync(int productId, ProductUpdateDto updateDto)
        {
            if (productId <= 0)
                throw new ArgumentException("Product Id must be greater than 0", nameof(productId));

            // Check if product exists
            var existingProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
                throw new KeyNotFoundException(ValidationMessages.ProductNotFound);

            var product = new Product
            {
                ProductId = productId,
                Name = updateDto.Name.Trim(),
                Price = updateDto.Price
            };

            bool success = await _unitOfWork.ProductRepository.UpdateProductAsync(product);

            if (!success)
                throw new InvalidOperationException("Failed to update product");

            // Fetch and return the updated product
            var updatedProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            if (updatedProduct == null)
                throw new InvalidOperationException("Failed to retrieve updated product");

            return MapToReadDto(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product Id must be greater than 0", nameof(productId));

            // Check if product exists
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException(ValidationMessages.ProductNotFound);

            bool success = await _unitOfWork.ProductRepository.DeleteProductAsync(productId);

            if (!success)
                throw new InvalidOperationException("Failed to delete product");

            return success;
        }

        private static ProductReadDto MapToReadDto(Product product)
        {
            return new ProductReadDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}

using ProductManagment.Domain.DTOs;

namespace ProductManagment.Core.Services
{
    public interface IProductService
    {
        Task<ProductReadDto> GetProductAsync(int productId);

        Task<List<ProductReadDto>> GetAllProductsAsync();

        Task<ProductReadDto> CreateProductAsync(ProductCreateDto createDto);

        Task<ProductReadDto> UpdateProductAsync(int productId, ProductUpdateDto updateDto);

        Task<bool> DeleteProductAsync(int productId);
    }
}

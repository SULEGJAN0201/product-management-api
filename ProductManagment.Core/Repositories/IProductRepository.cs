using ProductManagment.Domain.Entities;

namespace ProductManagment.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int productId);

        Task<List<Product>> GetAllProductsAsync();

        Task<int> CreateProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(int productId);
    }
}

using Microsoft.Extensions.Configuration;
using ProductManagment.Core.Repositories;
using ProductManagment.Infrastructure.Repositories;

namespace ProductManagment.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work implementation managing repositories and transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private IProductRepository? _productRepository;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_configuration);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            // For stored procedures, this is a placeholder for consistency
            // In real scenarios with transactions, this would commit the transaction
            return await Task.FromResult(0);
        }
    }
}

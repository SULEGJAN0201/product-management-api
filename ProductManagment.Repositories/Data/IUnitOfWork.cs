using ProductManagment.Core.Repositories;

namespace ProductManagment.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work interface for managing repositories and transactions
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Product repository
        /// </summary>
        IProductRepository ProductRepository { get; }

        /// <summary>
        /// Save all changes to the data store
        /// </summary>
        /// <returns>Number of affected records</returns>
        Task<int> SaveChangesAsync();
    }
}

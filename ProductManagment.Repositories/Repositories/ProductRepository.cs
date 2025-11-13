using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductManagment.Core.Repositories;
using ProductManagment.Domain.Entities;
using System.Data;

namespace ProductManagment.Infrastructure.Repositories
{
    /// <summary>
    /// Product repository implementation using SQL Server stored procedures
    /// </summary>
    public sealed class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("sp_GetProductById", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", productId);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapReaderToProduct(reader);
            }

            return null;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("sp_GetAllProducts", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(MapReaderToProduct(reader));
            }

            return products;
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("sp_InsertProduct", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("sp_UpdateProduct", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);

            // Add return value parameter
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.ParameterName = "@ReturnValue";
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnParameter);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            int returnValue = (int)command.Parameters["@ReturnValue"].Value;
            return returnValue > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("sp_DeleteProduct", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", productId);

            // Add return value parameter
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.ParameterName = "@ReturnValue";
            returnParameter.SqlDbType = SqlDbType.Int;
            returnParameter.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnParameter);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            int returnValue = (int)command.Parameters["@ReturnValue"].Value;
            return returnValue > 0;
        }

        /// <summary>
        /// Map SqlDataReader to Product entity
        /// </summary>
        private static Product MapReaderToProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = (int)reader["ProductId"],
                Name = reader["Name"].ToString() ?? "",
                Price = (decimal)reader["Price"]
            };
        }
    }
}

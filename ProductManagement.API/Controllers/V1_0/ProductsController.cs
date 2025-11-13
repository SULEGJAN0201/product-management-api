using Microsoft.AspNetCore.Mvc;
using ProductManagment.Core.Services;
using ProductManagment.Domain.DTOs;

namespace ProductManagement.API.Controllers.V1_0
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all products</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProductReadDto>>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Fetching all products");
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving products" });
            }
        }

        /// <summary>
        /// Get a product by ProductId
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>Product details</returns>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductReadDto>> GetProduct(int productId)
        {
            try
            {
                _logger.LogInformation("Fetching product with ProductId: {ProductId}", productId);
                var product = await _productService.GetProductAsync(productId);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Product not found with ProductId: {ProductId}", productId);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid input: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with ProductId: {ProductId}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving the product" });
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="createDto">Product create data</param>
        /// <returns>Newly created product object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductReadDto>> CreateProduct([FromBody] ProductCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new product: {ProductName}", createDto.Name);
                var createdProduct = await _productService.CreateProductAsync(createDto);
                return CreatedAtAction(nameof(GetProduct), new { productId = createdProduct.ProductId }, createdProduct);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating the product" });
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="updateDto">Product update data</param>
        /// <returns>Updated product details</returns>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(int productId, [FromBody] ProductUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating product with ProductId: {ProductId}", productId);
                var updatedProduct = await _productService.UpdateProductAsync(productId, updateDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Product not found with ProductId: {ProductId}", productId);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ProductId: {ProductId}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the product" });
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>Success message on deletion</returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                _logger.LogInformation("Deleting product with ProductId: {ProductId}", productId);
                await _productService.DeleteProductAsync(productId);
                return Ok(new { message = "Product deleted successfully", productId = productId });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Product not found with ProductId: {ProductId}", productId);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid input: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ProductId: {ProductId}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the product" });
            }
        }
    }
}

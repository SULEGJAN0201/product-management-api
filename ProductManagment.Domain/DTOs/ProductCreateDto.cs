using System.ComponentModel.DataAnnotations;

namespace ProductManagment.Domain.DTOs
{
    /// <summary>
    /// DTO for creating a new product
    /// </summary>
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Product name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}

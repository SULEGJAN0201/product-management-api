namespace ProductManagment.Domain.DTOs
{
    public class ProductReadDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}

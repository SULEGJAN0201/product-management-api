namespace ProductManagment.Domain.Entities
{
    /// <summary>
    /// Product entity representing a product in the system
    /// </summary>
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}

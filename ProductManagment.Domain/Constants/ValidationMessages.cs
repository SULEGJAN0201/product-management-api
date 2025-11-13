namespace ProductManagment.Domain.Constants
{
    /// <summary>
    /// Centralized validation and error messages
    /// </summary>
    public static class ValidationMessages
    {
        // Product validation messages
        public const string ProductNameRequired = "Product name is required";
        public const string ProductNameLength = "Product name must not exceed 100 characters";
        public const string ProductPriceRequired = "Price is required";
        public const string ProductPriceRange = "Price must be greater than 0";

        // Product operation messages
        public const string ProductNotFound = "Product not found";
        public const string ProductCreatedSuccessfully = "Product created successfully";
        public const string ProductUpdatedSuccessfully = "Product updated successfully";
        public const string ProductDeletedSuccessfully = "Product deleted successfully";

        // Error messages
        public const string InternalServerError = "An internal server error occurred. Please try again later";
        public const string InvalidInput = "Invalid input provided";
    }
}

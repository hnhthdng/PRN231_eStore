using eStoreClient.DTO.Product;
using Refit;

namespace eStoreClient.Services
{
    public interface IProductService
    {
        // Get all products
        [Get("/api/product")]
        Task<List<ProductResponseDTO>> GetAllProductsAsync();

        // Get a product by its ID
        [Get("/api/product/{id}")]
        Task<ProductResponseDTO> GetProductByIdAsync(int id);

        // Search products by name or unit price
        [Get("/api/product/search")]
        Task<List<ProductResponseDTO>> SearchProductsAsync([Query] string? name, [Query] int? unitPrice);

        // Get products by category ID
        [Get("/api/product/category/{categoryId}")]
        Task<List<ProductResponseDTO>> GetProductsByCategoryAsync(int categoryId);

        // Create a new product
        [Post("/api/product")]
        Task CreateProductAsync([Body] ProductRequestDTO productRequest);

        // Update an existing product
        [Put("/api/product/{id}")]
        Task UpdateProductAsync(int id, [Body] ProductRequestDTO productRequest);

        // Delete a product by its ID
        [Delete("/api/product/{id}")]
        Task DeleteProductAsync(int id);
    }
}

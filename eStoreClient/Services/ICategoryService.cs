using eStoreClient.DTO.Category;
using Refit;

namespace eStoreClient.Services
{
    public interface ICategoryService
    {
        [Get("/api/category")]
        Task<IEnumerable<CategoryResponseDTO>> GetCategories();

        [Get("/api/category/{id}")]
        Task<CategoryResponseDTO> GetCategoryById(int id);

        [Post("/api/category")]
        Task<CategoryRequestDTO> AddCategory([Body] CategoryRequestDTO categoryDTO);

        [Put("/api/category/{id}")]
        Task<int> UpdateCategory(int id, [Body] CategoryRequestDTO categoryDTO);

        [Delete("/api/category/{id}")]
        Task<int> DeleteCategory(int id);
    }
}

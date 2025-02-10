using BusinessObject.Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        int AddCategory(Category category);
        Category GetCategoryById(int id);
        List<Category> GetCategories();
        int UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}

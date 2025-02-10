using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepositories;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;  
        public CategoryRepository(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public int AddCategory(Category category)
        {
            _categoryDAO.AddCategory(category);
            return category.CategoryId;
        }

        public void DeleteCategory(int id)
        {
            _categoryDAO.DeleteCategory(id);
        }

        public List<Category> GetCategories()
        {
            return _categoryDAO.GetCategories();
        }

        public Category GetCategoryById(int id)
        {
            return _categoryDAO.FindCategoryById(id);
        }

        public int UpdateCategory(Category category)
        {
            _categoryDAO.UpdateCategory(category);
            return category.CategoryId;
        }
    }
}

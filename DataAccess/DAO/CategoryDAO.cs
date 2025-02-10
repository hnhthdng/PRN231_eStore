using BusinessObject.Models;
using DataAccess.Data;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        private readonly StoreDbContext _context;

        // Constructor nhận StoreDbContext từ DI
        public CategoryDAO(StoreDbContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                listCategories = _context.Categories.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listCategories;
        }

        public Category FindCategoryById(int categoryID)
        {
            var category = _context.Categories.Find(categoryID);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public int AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return category.CategoryId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                var categoryToUpdate = _context.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.CategoryName = category.CategoryName;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
                if (categoryToDelete != null)
                {
                    _context.Categories.Remove(categoryToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

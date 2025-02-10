using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepositories;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDAO;

        public ProductRepository(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }

        public void AddProduct(Product product) => _productDAO.AddProduct(product);

        public Product GetProductById(int id) => _productDAO.FindProductById(id);

        public List<Product> GetProducts() => _productDAO.GetProducts();

        public List<Product> Search(string? name, int? unitPrice) => _productDAO.Search(name, unitPrice);
        public List<Product> FindAllProductsByCategoryId(int categoryId) => _productDAO.FindAllProductsByCategoryId(categoryId);    

        public void UpdateProduct(Product product) => _productDAO.UpdateProduct(product);

        public void DeleteProduct(Product product) => _productDAO.DeleteProduct(product);
    }
}

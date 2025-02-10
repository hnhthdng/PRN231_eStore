using BusinessObject.Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface IProductRepository
    {
        void AddProduct(Product Product);
        Product GetProductById(int id);
        List<Product> GetProducts();
        List<Product> Search(string? name, int? unitPrice);
        void UpdateProduct(Product Product);
        void DeleteProduct(Product Product);
        public List<Product> FindAllProductsByCategoryId(int categoryId);
    }
}

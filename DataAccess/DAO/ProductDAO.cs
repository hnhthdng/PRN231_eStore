using BusinessObject.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        private readonly StoreDbContext _context;

        // Constructor nhận StoreDbContext từ DI
        public ProductDAO(StoreDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                listProducts = _context.Products.ToList();
                listProducts.ForEach(f =>
                {
                    f.Category = _context.Categories.Find(f.CategoryId);
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }

        public List<Product> Search(string? name, int? unitPrice)
        {
            var listProducts = new List<Product>();
            try
            {
                // Build the query dynamically based on provided parameters
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(f => f.ProductName.Contains(name));
                }

                if (unitPrice.HasValue)
                {
                    query = query.Where(f => f.UnitPrice <= unitPrice.Value);
                }

                listProducts = query.ToList();

                // Populate related categories
                listProducts.ForEach(f =>
                {
                    f.Category = _context.Categories.Find(f.CategoryId);
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }


        public List<Product> FindAllProductsByCategoryId(int categoryId)
        {
            var listProducts = new List<Product>();
            try
            {
                listProducts = _context.Products
                    .Where(f => f.CategoryId == categoryId)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }

        public Product FindProductById(int productId)
        {
            Product product;
            try
            {
                product = _context.Products
                    .SingleOrDefault(f => f.ProductId == productId);
                if (product != null)
                {
                    product.Category = _context.Categories.Find(product.CategoryId);
                    product.OrderDetails = _context.OrderDetails
                        .Where(o => o.ProductId == productId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                var productToUpdate = _context.Products
                    .SingleOrDefault(f => f.ProductId == product.ProductId);
                if (productToUpdate != null)
                {
                    productToUpdate.ProductName = product.ProductName;
                    productToUpdate.CategoryId = product.CategoryId;
                    productToUpdate.UnitPrice = product.UnitPrice;
                    productToUpdate.Weight = product.Weight;
                    productToUpdate.UnitsInStock = product.UnitsInStock;
                    productToUpdate.CategoryId = product.CategoryId;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteProduct(Product product)
        {
            try
            {
                var productToDelete = _context.Products
                    .SingleOrDefault(f => f.ProductId == product.ProductId);
                if (productToDelete != null)
                {
                    _context.Products.Remove(productToDelete);
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

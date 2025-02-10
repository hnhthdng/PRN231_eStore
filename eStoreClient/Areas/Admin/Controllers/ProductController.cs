using eStoreClient.DTO.Product;
using eStoreClient.Models;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string searchName, int? searchPrice)
        {
            IEnumerable<ProductResponseDTO> products = await _productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(searchName))
            {
                products = products.Where(p => p.ProductName.Contains(searchName, StringComparison.OrdinalIgnoreCase));
            }

            if (searchPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= searchPrice.Value);
            }

            List<ProductViewModel> productsViewModel = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var category = await _categoryService.GetCategoryById(product.CategoryId);
                productsViewModel.Add(new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    Weight = product.Weight,
                    UnitsInStock = product.UnitsInStock,
                    CategoryName = category.CategoryName
                });
            }

            return View(productsViewModel);
        }

        //Create
        public async Task<IActionResult> CreateAsync()
        {
            var product = new ProductRequestDTO();

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequestDTO product)
        {
            if (!ModelState.IsValid)
            {
                // Load lại danh sách Category nếu ModelState không hợp lệ
                var categories = await _categoryService.GetCategories();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

                return View(product);
            }

            await _productService.CreateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }


        //Update
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName,
                Selected = (c.CategoryId == product.CategoryId) // Tự động chọn category hiện tại
            }).ToList();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductResponseDTO product)
        {
            if (ModelState.IsValid)
            {
                var productDTO = new ProductRequestDTO
                {
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    Weight = product.Weight,
                    UnitsInStock = product.UnitsInStock,
                    CategoryId = product.CategoryId
                };
                await _productService.UpdateProductAsync(product.ProductId, productDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

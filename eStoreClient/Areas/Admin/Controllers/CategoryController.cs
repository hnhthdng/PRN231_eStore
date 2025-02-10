using eStoreClient.DTO.Category;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace eStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories().Result;
            return View(categories);
        }

        //Create
        public IActionResult Create()
        {
            var category = new CategoryRequestDTO();
            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequestDTO category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //Update
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryResponseDTO category)
        {
            if (ModelState.IsValid)
            {
                var categoryRequest = new CategoryRequestDTO
                {
                    CategoryName = category.CategoryName
                };
                await _categoryService.UpdateCategory(category.CategoryId, categoryRequest);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

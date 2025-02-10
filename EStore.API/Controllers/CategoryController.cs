using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Category;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryRepository _categoryRepository;
        IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get() {
            var categories = _categoryRepository.GetCategories();
            var categoriesDTO = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
            return Ok(categoriesDTO);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            var categoryDTO = _mapper.Map<CategoryResponseDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoryRequestDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            _categoryRepository.AddCategory(category);
            return Ok(categoryDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryRequestDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            category.CategoryId = id;
            _categoryRepository.UpdateCategory(category);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isExistCategory = _categoryRepository.GetCategoryById(id);
            if (isExistCategory == null)
            {
                return NotFound();
            }
            _categoryRepository.DeleteCategory(id);
            return Ok(id);
        }
    }
}

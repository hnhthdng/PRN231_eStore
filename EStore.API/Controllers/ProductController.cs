using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Product;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts();
            var productsResponse = _mapper.Map<List<ProductResponseDTO>>(products);
            return Ok(productsResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var productResponse = _mapper.Map<ProductResponseDTO>(product);
            return Ok(productResponse);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? name, [FromQuery] int? unitPrice)
        {
            var products = _productRepository.Search(name, unitPrice);
            var productsResponse = _mapper.Map<List<ProductResponseDTO>>(products);
            return Ok(productsResponse);
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _productRepository.FindAllProductsByCategoryId(categoryId);
            var productsResponse = _mapper.Map<List<ProductResponseDTO>>(products);
            return Ok(productsResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductRequestDTO productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);
            _productRepository.AddProduct(product);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductRequestDTO productRequest)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            _mapper.Map(productRequest, product);
            _productRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepository.DeleteProduct(product);
            return Ok();
        }
    }
}

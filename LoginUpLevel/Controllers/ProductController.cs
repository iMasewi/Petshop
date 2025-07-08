using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Text.Json;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving product: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                if (products == null || !products.Any())
                {
                    return NotFound("No products found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving products: {ex.Message}");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(string category)
        {
            try
            {
                var products = await _productService.GetProductByCategoryAsync(category);
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found in category '{category}'");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving products by category: {ex.Message}");
            }
        }

        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySuggestion([FromQuery]string name)
        {
            try
            {
                var products = await _productService.GetproductByNameAsync(name);
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found in category '{name}'");
                }
                return Ok(products);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> PostProduct([FromForm] ProductDTO productDto, IFormFile? image)
        {
            try
            {
                if (productDto == null)
                {
                    return BadRequest("Product data is null");
                }
                if (!string.IsNullOrEmpty(productDto.ProductColorsJson))
                {
                    productDto.ProductColors = JsonSerializer.Deserialize<ICollection<ProductColorDTO>>(
                        productDto.ProductColorsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                }
                var createdProduct = await _productService.CreateAsync(productDto, image);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating product: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO updateDto,IFormFile? image)
        {
            try
            {
                if (updateDto == null)
                {
                    return BadRequest("Product data is invalid");
                }
                if (!string.IsNullOrEmpty(updateDto.ProductColorsJson))
                {
                    updateDto.ProductColors = JsonSerializer.Deserialize<ICollection<ProductColorDTO>>(
                        updateDto.ProductColorsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                }
                await _productService.UpdateAsync(id, updateDto, image);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting product: {ex.Message}");
            }
        }
    }
}

using LoginUpLevel.DTOs;
using LoginUpLevel.Models;

namespace LoginUpLevel.Services
{
    public interface IProductService
    {
        Task<ProductDTO> CreateAsync(ProductDTO productDto, IFormFile image);
        Task UpdateAsync(int id, ProductDTO updateDto, IFormFile? image);
        Task DeleteAsync(int id);
        Task<ProductDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllAsync(int page, int pageSize);
        Task<IEnumerable<ProductDTO>> GetProductByCategoryAsync(string category);
        Task<IEnumerable<ProductDTO>> GetproductByNameAsync(string name);
    }
}
using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IProductColorRepository : IRepository<ProductColor>
    {
        Task<IEnumerable<ProductColor>> GetProductColorsByProductIdAsync(int productId);
        Task<ProductColor> GetProductColorByProductAndColorAsync(int productId, int colorId);
        Task<IEnumerable<ProductColor>> GetProductColorsByColorIdAsync(int colorId);
        Task<bool> IsProductColorExistsAsync(int productId, int colorId);
    }
}

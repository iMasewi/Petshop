using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;

namespace LoginUpLevel.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> SearchByNameAsync(string name);
        //Task<IEnumerable<Product>> SearchByColorAsync(string color);
        Task<IEnumerable<Product>> GetProductByCatagoryAsync(string category);
    }
}
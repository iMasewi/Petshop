using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Product>> GetProductByCatagoryAsync(string category)
        {
            return _context.Products
                .Where(p => p.Category != null && p.Category.Contains(category))
                .ToListAsync()
                .ContinueWith(task => task.Result.AsEnumerable());
        }

        //public Task<IEnumerable<Product>> SearchByColorAsync(string color)
        //{
        //    return _context.Products
        //        .Where(p => p.Color.Contains(color))
        //        .ToListAsync()
        //        .ContinueWith(task => task.Result.AsEnumerable());
        //}

        public Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            return _context.Products
                .Where(p => p.Name.Contains(name))
                .ToListAsync()
                .ContinueWith(task => task.Result.AsEnumerable());
        }
    }
}

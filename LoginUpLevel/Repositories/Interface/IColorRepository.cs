using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IColorRepository : IRepository<Color>
    {
        Task<bool> IsColorExistsAsync(int colorId);
        Task<int> CountColor();
    }
}

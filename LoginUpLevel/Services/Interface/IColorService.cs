using LoginUpLevel.DTOs;
using LoginUpLevel.Models;

namespace LoginUpLevel.Services.Interface
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDTO>> GetAllColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task AddColorAsync(ColorDTO colorDto);
        Task UpdateColorAsync(ColorDTO colorDto);
        Task DeleteColorAsync(int id);
        Task<int> CountColorsAsync();
    }
}

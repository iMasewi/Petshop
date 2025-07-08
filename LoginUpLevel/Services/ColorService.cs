using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;

namespace LoginUpLevel.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddColorAsync(ColorDTO colorDto)
        {
            try
            {
                if(await _unitOfWork.ColorRepository.IsColorExistsAsync(colorDto.Id))
                {
                    throw new Exception($"Color with name {colorDto.NameColor} already exists.");
                }
                var color = _mapper.Map<Color>(colorDto);
                if (color == null)
                {
                    throw new ArgumentNullException(nameof(colorDto), "Color cannot be null");
                }
                await _unitOfWork.ColorRepository.Add(color);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the color.", ex);
            }
        }
        public async Task<int> CountColorsAsync()
        {
            try
            {
                return await _unitOfWork.ColorRepository.Count();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while counting colors.", ex);
            }
        }

        public async Task DeleteColorAsync(int id)
        {
            try
            {
                var color = await _unitOfWork.ColorRepository.GetById(id);
                if (color == null)
                {
                    throw new Exception($"Color with ID {id} not found.");
                }
                await _unitOfWork.ColorRepository.Delete(color);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the color.", ex);
            }
        }

        public async Task<IEnumerable<ColorDTO>> GetAllColorsAsync()
        {
            try
            {
                var colors = await _unitOfWork.ColorRepository.GetAll();
                if (colors == null)
                {
                    throw new Exception($"Invalid colors");
                }
                return _mapper.Map<IEnumerable<ColorDTO>>(colors);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all colors.", ex);
            }
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            try
            {
                var color = await _unitOfWork.ColorRepository.GetById(id);
                if (color == null)
                {
                    throw new Exception($"Invalid colors");
                }
                return color;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all colors.", ex);
            }
        }

        public async Task UpdateColorAsync(ColorDTO colorDto)
        {
            try
            {
                var color = await _unitOfWork.ColorRepository.GetById(colorDto.Id);
                if (color == null)
                {
                    throw new Exception($"Color with ID {colorDto.Id} not found.");
                }
                MapProductData(colorDto, color);
                await _unitOfWork.ColorRepository.Update(color);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the color.", ex);

            }
        }
        private void MapProductData(ColorDTO dto, Color entity)
        {
            if (dto.NameColor != null && dto.NameColor == "") entity.NameColor = dto.NameColor;
        }
    }
}

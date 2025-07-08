using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;

        public ColorController(IColorService colorService, IMapper mapper)
        {
            _colorService = colorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<ColorDTO>> GetAllColor()
        {
            try
            {
                var colors = await _colorService.GetAllColorsAsync();
                return Ok(colors);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<ColorDTO>> GetColor(int id)
        {
            try
            {
                var color = await _colorService.GetColorByIdAsync(id);
                if (color == null)
                {
                    return NotFound($"Color with ID {id} not found.");
                }
                return Ok(color);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> CreateColor([FromBody] ColorDTO colorDto)
        {
            try
            {
                if (colorDto == null)
                {
                    return BadRequest("Color data is null.");
                }
                await _colorService.AddColorAsync(colorDto);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] ColorDTO colorDto)
        {
            try
            {
                if (colorDto == null)
                {
                    return BadRequest("Color data is null.");
                }
                var existingColor = await _colorService.GetColorByIdAsync(id);
                if (existingColor == null)
                {
                    return NotFound($"Color with ID {id} not found.");
                }
                colorDto.Id = id;
                await _colorService.UpdateColorAsync(colorDto);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {
                var existingColor = await _colorService.GetColorByIdAsync(id);
                if (existingColor == null)
                {
                    return NotFound($"Color with ID {id} not found.");
                }
                await _colorService.DeleteColorAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

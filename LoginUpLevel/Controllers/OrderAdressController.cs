using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderAdressController : ControllerBase
    {
        private readonly IOrderAdressService _orderAdressService;
        private readonly IMapper _mapper;
        public OrderAdressController(IOrderAdressService orderAdressService, IMapper mapper)
        {
            _orderAdressService = orderAdressService;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderAdressDTO>> GetOrderAdressByIdAsync(int id)
        {
            try
            {
                var orderAdress = await _orderAdressService.GetOrderAdressByIdAsync(id);

                if (orderAdress == null)
                {
                    return NotFound($"Order address with ID {id} not found.");
                }

                return Ok(orderAdress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding order address: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OrderAdressDTO>> GetAllOrderAdressAsync()
        {
            try
            {
                var orderAdress = await _orderAdressService.GetAllOrderAdressAsync();

                if (orderAdress == null)
                {
                    return NotFound($"Invalid adress");
                }

                return Ok(orderAdress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding order address: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrderAdressAsync(OrderAdressDTO orderAdressDto)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(id, out int customerId))
                {
                    return BadRequest("Invalid customer ID.");
                }

                orderAdressDto.CustomerId = customerId;

                var result = await _orderAdressService.AddOrderAdressAsync(orderAdressDto);

                if (result == null)
                {
                    return BadRequest("Failed to add order address.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding order address: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderAdressAsync(int id, OrderAdressDTO orderAdressDto)
        {
            try
            {
                int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (id != orderAdressDto.Id)
                {
                    return BadRequest("Order address ID mismatch.");
                }
                await _orderAdressService.UpdateOrderAdressAsync(orderAdressDto, id, customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating order address: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrderAdressAsync(int id)
        {
            try
            {
                int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                await _orderAdressService.DeleteOrderAdressAsync(id, customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting order address: {ex.Message}");
            }
        }
    }
}

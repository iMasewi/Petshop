using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving order: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders: {ex.Message}");
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles ="Customer")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByCustomerId()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier) ? .Value;

                if (!int.TryParse(id, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }

                var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders: {ex.Message}");
            }
        }

        [HttpGet("Admin/{statusId}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByStatusId(int statusId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByStatusIdAsync(statusId);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PostOrder([FromBody]OrderDTO orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    return BadRequest("Order data is null");
                }

                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(id, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }

                var createdOrder = await _orderService.CreateAsync(orderDto, customerId);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> PutProduct(int id, [FromQuery] int statusId)
        {
            try
            {
                await _orderService.UpdateStatusAsync(id, statusId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating order status: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting order: {ex.Message}");
            }
        }
    }
}

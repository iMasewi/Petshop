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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItemsAsync()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(id, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }

                return Ok(await _cartService.GetCartByCustomerIdAsync(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CartItemDTO>> AddToCartAsync([FromBody] CartItemDTO cartItemDto)
        {
            try
            {
                if (cartItemDto == null)
                {
                    return BadRequest("Cart item cannot be null");
                }
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(id, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }
                await _cartService.AddToCartAsync(cartItemDto, customerId);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItemAsync(int productId)
        {
            try
            {
                await _cartService.DeleteCartItemAsync(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] CartItemDTO cartItemDto)
        {
            try
            {
                if (cartItemDto == null)
                {
                    return BadRequest("Cart item cannot be null");
                }
                await _cartService.UpdateCartAsync(cartItemDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("order")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> OrderInCartAsync([FromBody] OrderDTO orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    return BadRequest("Order cannot be null");
                }
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(id, out var customerId))
                {
                    return BadRequest("Invalid customer ID in user claims.");
                }
                orderDto.CustomerId = customerId;
                await _cartService.OrderInCart(orderDto, customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

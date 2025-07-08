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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return BadRequest("Invalid Customer");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving customer: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<CustomerDTO>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                if (customers == null || !customers.Any())
                {
                    return NotFound();
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving customers: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromForm] CustomerDTO customerDto)
        {
            try
            {
                var customer = _customerService.CheckDuplicateCustomerAsync(customerDto.Email, customerDto.Username);
                if (customer == null)
                {
                    return BadRequest("Invalid Employee");
                }
                var result = await _customerService.AddCustomerAsync(customerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating customer: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCustomer([FromForm] CustomerDTO customerDTO, int id)
        {
            try
            {
                int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (customerId != id)
                {
                    return BadRequest("You can only update your own profile.");
                }

                var employee = _mapper.Map<CustomerDTO>(customerDTO);
                if (employee == null)
                {
                    return BadRequest("Invalid Employee");
                }
                await _customerService.UpdateCustomerAsync(employee, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting employee: {ex.Message}");
            }
        }
    }
}

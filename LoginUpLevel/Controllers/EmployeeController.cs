using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving employee: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                if (employees == null)
                {
                    return NotFound();
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving employees: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee([FromForm] EmployeeDTO employeeDto)
        {
            var checkDuplicate = await _employeeService.CheckDuplicateEmployeeAsync(employeeDto.Email, employeeDto.Username);
            if (checkDuplicate)
            {
                return BadRequest("Email or username already exists");
            }
            var createdEmployee = await _employeeService.AddEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<EmployeeDTO>> PutEmployee(int id, [FromForm] EmployeeDTO employeeDto)
        {
            try
            {
                var checkDuplicate = await _employeeService.CheckDuplicateEmployeeAsync(employeeDto.Email, employeeDto.Username, id);
                if (checkDuplicate)
                {
                    return BadRequest("Email or username already exists");
                }
                await _employeeService.UpdateEmployeeAsync(id, employeeDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting employee: {ex.Message}");
            }
        }
    }
}

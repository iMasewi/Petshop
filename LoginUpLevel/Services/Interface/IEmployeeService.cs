using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO employeeDto);
        Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDto);
        Task DeleteEmployeeAsync(int id);
        Task<bool> CheckDuplicateEmployeeAsync(string email, string username);
        Task<bool> CheckDuplicateEmployeeAsync(string email, string username, int id);
    }
}

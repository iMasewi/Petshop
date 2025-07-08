using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using LoginUpLevel.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LoginUpLevel.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO employeeDto)
        {
            try
            {
                var newEmployee = _mapper.Map<Employee>(employeeDto);
                var createUser = await _userManager.CreateAsync(newEmployee, employeeDto.PasswordHash);
                if (!createUser.Succeeded)
                {
                    throw new Exception("Failed to create user: ");
                }
                else
                {
                    if (employeeDto.StaffRole == 0)
                    {
                        await _userManager.AddToRoleAsync(newEmployee, "Manager");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newEmployee, "Employee");
                    }

                    return employeeDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add employee", ex);
            }
        }

        public Task<bool> CheckDuplicateEmployeeAsync(string email, string username)
        {
            return _unitOfWork.EmployeeRepository.CheckDuplicateEmployee(email, username);
        }

        public Task<bool> CheckDuplicateEmployeeAsync(string email, string username, int id)
        {
            return _unitOfWork.EmployeeRepository.CheckDuplicateEmployee(email, username, id);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }
            await _unitOfWork.EmployeeRepository.Delete(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAll();
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            foreach (var employee in employees)
            {
                var role = await _userManager.GetRolesAsync(employee);
                if (role.Count > 0)
                {
                    if (role[0] == "Manager")
                    {
                        employeesDto.FirstOrDefault(x => x.Id == employee.Id).StaffRole = 0;
                    }
                    else
                    {
                        employeesDto.FirstOrDefault(x => x.Id == employee.Id).StaffRole = 1;
                    }
                    employeesDto.FirstOrDefault(x => x.Id == employee.Id).Role = role[0];
                }
            }
            return employeesDto;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            var employeeDto = _mapper.Map<EmployeeDTO>(employee);
            var role = await _userManager.GetRolesAsync(employee);
            if (role.Count > 0)
            {
                if (role[0] == "Manager")
                {
                    employeeDto.StaffRole = 0;
                }
                else
                {
                    employeeDto.StaffRole = 1;
                }
                employeeDto.Role = role[0];
            }
            return employeeDto;
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeDTO employeeDto)
        {
            try
            {
                var oldEmployee = await _unitOfWork.EmployeeRepository.GetById(id);
                if (oldEmployee == null)
                {
                    throw new Exception("Employee not found");
                }
                if (string.IsNullOrEmpty(employeeDto.PasswordHash))
                {
                    
                }
                else
                {
                    MapEmployeeData(employeeDto, oldEmployee);
                    var newPasswordHash = _userManager.PasswordHasher.HashPassword(oldEmployee, employeeDto.PasswordHash);
                    oldEmployee.PasswordHash = newPasswordHash;
                }
                await _userManager.UpdateAsync(oldEmployee);

                var userRole = await _userManager.GetRolesAsync(oldEmployee);
                await _userManager.RemoveFromRolesAsync(oldEmployee, userRole);
                
                if(employeeDto.StaffRole == 0)
                {
                    await _userManager.AddToRoleAsync(oldEmployee, "Manager");
                }
                else
                {
                    await _userManager.AddToRoleAsync(oldEmployee, "Employee");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update employee", ex);
            }
        }
        private void MapEmployeeData(EmployeeDTO dto, Employee entity)
        {
            if (dto.FirstName != null && dto.FirstName != "") entity.FirstName = dto.FirstName;
            if (dto.LastName != null && dto.LastName != "") entity.LastName = dto.LastName;
            if (dto.Email != null && dto.Email != "") entity.Email = dto.Email;
            if (dto.PhoneNumber != null && dto.PhoneNumber != "") entity.PhoneNumber = dto.PhoneNumber;
            if (dto.Username != null && dto.Username != "") entity.UserName = dto.Username;
            if (dto.Address != null && dto.Address != "") entity.Address = dto.Address;
            if (dto.Salary != 0) entity.Salary = dto.Salary;
        }

    }
}

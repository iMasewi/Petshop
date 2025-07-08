using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using LoginUpLevel.Utils;
using Microsoft.AspNetCore.Identity;

namespace LoginUpLevel.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO customerDto)
        {
            try
            {
                var newCustomer = _mapper.Map<Customer>(customerDto);

                var createUser = await _userManager.CreateAsync(newCustomer, customerDto.PasswordHash);
                if (!createUser.Succeeded)
                {
                    throw new Exception("Failed to create user");
                }
                else
                {
                    var userRole = await _userManager.AddToRoleAsync(newCustomer, "Customer");
                    if (!userRole.Succeeded)
                    {
                        throw new Exception("Failed to add user to role: ");
                    }
                    
                    //Add Cart
                    var newCart = new Cart
                    {
                        CustomerId = newCustomer.Id,
                        TotalPrice = 0
                    };
                    await _unitOfWork.CartRepository.Add(newCart);
                    await _unitOfWork.SaveChangesAsync();

                    var customer = _mapper.Map<CustomerDTO>(newCustomer);
                    return customer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add customer" + ex);
            }
        }

        public Task<bool> CheckDuplicateCustomerAsync(string email, string username)
        {
            return _unitOfWork.CustomerRepository.CheckDuplicateCustomer(email, username);
        }

        public Task<bool> CheckDuplicateCustomerAsync(string email, string username, int id)
        {
            return _unitOfWork.CustomerRepository.CheckDuplicateCustomer(email, username, id); 
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(id);
            var orders = await _unitOfWork.OrderRepository.GetOrdersByCustomerIdAsync(customer.Id);
            var orderAdress = await _unitOfWork.OrderAdressRepository.GetOrderAdressByCustomer(customer.Id);

            foreach(var adress in orderAdress)
            {
                await _unitOfWork.OrderAdressRepository.Delete(adress);
                await _unitOfWork.SaveChangesAsync();
            }

            foreach(var order in orders)
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetOrderDetailsByOrderIdAsync(order.Id);
                foreach (var detail in orderDetails)
                {
                    await _unitOfWork.OrderDetailRepository.Delete(detail);
                    await _unitOfWork.SaveChangesAsync();
                }
                await _unitOfWork.OrderRepository.Delete(order);
                await _unitOfWork.SaveChangesAsync();
            }

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            await _unitOfWork.CustomerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers =  await _unitOfWork.CustomerRepository.GetAll();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetById(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task UpdateCustomerAsync(CustomerDTO customerDto, int id)
        {
            try
            {
                var oldCustomer = await _unitOfWork.CustomerRepository.GetById(id);
                if (oldCustomer == null)
                {
                    throw new Exception("Customer not found");
                }

                if (string.IsNullOrEmpty(customerDto.PasswordHash))
                {
                    MapCustomerData(customerDto, oldCustomer);
                }
                else
                {
                    MapCustomerData(customerDto, oldCustomer);
                    var newPasswordHash = _userManager.PasswordHasher.HashPassword(oldCustomer, customerDto.PasswordHash);
                    oldCustomer.PasswordHash = newPasswordHash;
                }

                await _userManager.UpdateAsync(oldCustomer);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update customer", ex);
            }
        }
        private void MapCustomerData(CustomerDTO dto, Customer entity)
        {
            if (dto.FirstName != null && dto.FirstName != "") entity.FirstName = dto.FirstName;
            if (dto.LastName != null && dto.LastName != "") entity.LastName = dto.LastName;
            if (dto.Email != null && dto.Email != "") entity.Email = dto.Email;
            if (dto.PhoneNumber != null && dto.PhoneNumber != "") entity.PhoneNumber = dto.PhoneNumber;
            if (dto.Username != null && dto.Username != "") entity.UserName = dto.Username;
            if (dto.Address != null && dto.Address != "") entity.Address = dto.Address;
        }
    }
}
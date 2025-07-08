using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;

namespace LoginUpLevel.Services
{
    public class OrderAdressService : Interface.IOrderAdressService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderAdressService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderAdressDTO> AddOrderAdressAsync(OrderAdressDTO orderAdresssDto)
        {
            var orderAdress = _mapper.Map<OrderAdress>(orderAdresssDto);

            if (orderAdress == null)
            {
                throw new ArgumentNullException(nameof(orderAdresssDto), "Order address cannot be null.");
            }

            await _unitOfWork.OrderAdressRepository.Add(orderAdress);
            await _unitOfWork.SaveChangesAsync();
            return orderAdresssDto;
        }

        public async Task DeleteOrderAdressAsync(int id, int customerId)
        {
            var orderAdress = await _unitOfWork.OrderAdressRepository.GetById(id);
            if (orderAdress == null)
            {
                throw new Exception("Order address not found.");
            }

            if(orderAdress.CustomerId != customerId)
            {
                throw new Exception("you can only delete your own order detail");
            }
            await _unitOfWork.OrderAdressRepository.Delete(orderAdress);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderAdressDTO>> GetAllOrderAdressAsync()
        {
            var orderAddresses = await _unitOfWork.OrderAdressRepository.GetAll();
            if (orderAddresses == null)
            {
                throw new Exception("No order addresses found.");
            }
            return _mapper.Map<IEnumerable<OrderAdressDTO>>(orderAddresses);
        }

        public async Task<OrderAdressDTO> GetOrderAdressByIdAsync(int id)
        {
            var orderAddress = await _unitOfWork.OrderAdressRepository.GetById(id);
            if (orderAddress == null)
            {
                throw new Exception("No order addresses found.");
            }
            return _mapper.Map<OrderAdressDTO>(orderAddress);
        }

        public async Task UpdateOrderAdressAsync(OrderAdressDTO orderAdresssDto, int id, int customerId)
        {
            var orderAdress = await _unitOfWork.OrderAdressRepository.GetById(id);

            if (orderAdress == null)
            {
                throw new Exception("Order address not found.");
            }

            if(orderAdress.CustomerId != customerId)
            {
                throw new Exception("you can only update your own order adress");
            }

            MapCustomerData(orderAdresssDto, orderAdress);
            await _unitOfWork.OrderAdressRepository.Update(orderAdress);
            await _unitOfWork.SaveChangesAsync();
        }
        private void MapCustomerData(OrderAdressDTO dto, OrderAdress entity)
        {
            if(dto.Name != null && dto.Name != "") entity.Name = dto.Name;
            if (dto.Email != null && dto.Email != "") entity.Email = dto.Email;
            if (dto.PhoneNumber != null && dto.PhoneNumber != "") entity.PhoneNumber = dto.PhoneNumber;
            if (dto.Address != null && dto.Address != "") entity.Address = dto.Address;
        }
    }
}

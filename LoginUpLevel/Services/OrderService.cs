using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;

namespace LoginUpLevel.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateAsync(OrderDTO orderDto, int customerId)
        {
            try
            {
                var orderAdress = await _unitOfWork.OrderAdressRepository.GetById(orderDto.OrderAdressId);
                if (orderAdress == null)
                {
                    throw new Exception("Invalid Order Address");
                }

                var newOrder = new Order
                {
                    Name = orderAdress.Name,
                    PhoneNumber = orderAdress.PhoneNumber,
                    Address = orderAdress.Address,
                    Email = orderAdress.Email,
                    Note = orderDto.Note,
                    CustomerId = customerId
                };

                if (newOrder == null)
                {
                    throw new Exception("Failed to create order");
                }
                await _unitOfWork.OrderRepository.Add(newOrder);
                await _unitOfWork.SaveChangesAsync();

                float totalPrice = 0;

                if (orderDto.OrderItems.Count() > 0)
                {
                    foreach (var item in orderDto.OrderItems)
                    {
                        var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                        if (product == null)
                        {
                            throw new Exception("Invalid Product");
                        }

                        if(product.AMount < item.AMount)
                        {
                            throw new Exception($"Insufficient stock for product {product.Name}");
                        }

                        var orderDetail = new OrderDetail
                        {
                            TotalPrice = item.AMount * product.Price,
                            AMount = item.AMount,
                            OrderId = newOrder.Id,
                            ProductId = item.ProductId
                        };

                        await _unitOfWork.OrderDetailRepository.Add(orderDetail);
                        await _unitOfWork.SaveChangesAsync();

                        totalPrice += orderDetail.TotalPrice;
                        product.AMount -= item.AMount;
                    }
                }

                newOrder.TotalPrice = totalPrice;
                await _unitOfWork.OrderRepository.Update(newOrder);
                await _unitOfWork.SaveChangesAsync();
                return orderDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create order", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetById(id);
            if (order == null)
            {
                throw new Exception($"Order with ID {id} not found");
            }

            var orderDetail = await _unitOfWork.OrderDetailRepository.GetOrderDetailsByOrderIdAsync(id);
            foreach(var item in orderDetail)
            {
                await _unitOfWork.OrderDetailRepository.Delete(item);
            }
            await _unitOfWork.OrderRepository.Delete(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAll();
            if (orders == null)
            {
                throw new Exception("No orders found");
            }
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(id);
                if (order == null)
                {
                    throw new Exception($"Order with ID {id} not found");
                }

                var orderDto = _mapper.Map<OrderDTO>(order);
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetOrderDetailsByOrderIdAsync(id);

                foreach (var item in orderDetail)
                {
                    var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                    var orderDetailDto = new OrderDetailDTO
                    {
                        AMount = item.AMount,
                        TotalPrice = item.AMount * product.Price,
                        ProductId = item.ProductId,
                        OrderId = item.OrderId
                    };

                    orderDto.OrderItems.Add(orderDetailDto);
                }
                return orderDto;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByCustomerIdAsync(customerId);

            if(orders == null)
            {
                throw new Exception("Invalid orders");
            }
            var ordersDto = _mapper.Map<IEnumerable<OrderDTO>>(orders); 

            foreach(var orderDto in ordersDto)
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetOrderDetailsByOrderIdAsync(orderDto.Id);

                foreach (var item in orderDetail)
                {
                    var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                    var orderDetailDto = new OrderDetailDTO
                    {
                        AMount = item.AMount,
                        TotalPrice = item.AMount * product.Price,
                        ProductId = item.ProductId,
                        OrderId = item.OrderId
                    };
                    orderDto.OrderItems.Add(orderDetailDto);
                }
            }
            return ordersDto;
        }
        public async Task<IEnumerable<OrderDTO>> GetOrdersByStatusIdAsync(int customerId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByStatusAsync(customerId);

            if (orders == null)
            {
                throw new Exception("Invalid orders");
            }

            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task UpdateStatusAsync(int id, int statusId)
        {
            try
            {
                var oldorder = await _unitOfWork.OrderRepository.GetById(id);
                if (oldorder == null)
                {
                    throw new Exception($"Order with ID {id} not found");
                }
                oldorder.StatusId = statusId;
                oldorder.StatusName = statusId switch
                {
                    1 => "Pending",
                    2 => "Processing",
                    3 => "Completed",
                    4 => "Cancelled",
                    _ => throw new ArgumentException("Invalid status ID")
                };
                await _unitOfWork.OrderRepository.Update(oldorder);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update order status", ex);

            }
        }
    }
}

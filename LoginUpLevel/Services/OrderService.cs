﻿using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace LoginUpLevel.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string OrderCacheKey = "order_list";
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
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
                            AMount = item.AMount,
                            Price = product.Price,
                            TotalPrice = item.AMount * product.Price,
                            ProductName = product.Name,
                            ProductImage = product.Image,
                            OrderId = newOrder.Id,
                            ProductId = product.Id
                        };

                        await _unitOfWork.OrderDetailRepository.Add(orderDetail);
                        totalPrice += orderDetail.TotalPrice;
                        product.AMount -= item.AMount;
                    }
                }

                newOrder.TotalPrice = totalPrice;
                await _unitOfWork.OrderRepository.Update(newOrder);
                await _unitOfWork.SaveChangesAsync();

                _cache.Remove(OrderCacheKey);
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

            _cache.Remove(OrderCacheKey);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            if(_cache.TryGetValue(OrderCacheKey, out IEnumerable<OrderDTO> cachedOrder))
            {
                return cachedOrder;
            }

            var orders = await _unitOfWork.OrderRepository.GetAll();
            if (orders == null)
            {
                throw new Exception("No orders found");
            }

            var ordersDto = _mapper.Map<IEnumerable<OrderDTO>>(orders);

            _cache.Set(OrderCacheKey, ordersDto, TimeSpan.FromMinutes(30));

            return ordersDto;
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
                    var orderDetailDto = _mapper.Map<OrderDetailDTO>(item);

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
            var cacheKey = $"orders_by_customerId_{customerId}";
            if(_cache.TryGetValue(cacheKey, out IEnumerable<OrderDTO> cachedOrder))
            {
                return cachedOrder;
            }

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
                    var orderDetailDto = _mapper.Map<OrderDetailDTO>(item);
                    orderDto.OrderItems.Add(orderDetailDto);
                }
            }

            _cache.Set(cacheKey, ordersDto, TimeSpan.FromMinutes(30));
            return ordersDto;
        }
        public async Task<IEnumerable<OrderDTO>> GetOrdersByStatusIdAsync(int statusId)
        {
            var cacheKey = $"orders_by_statusId_{statusId}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<OrderDTO> cachedOrder))
            {
                return cachedOrder;
            }

            var orders = await _unitOfWork.OrderRepository.GetOrdersByStatusAsync(statusId);

            if (orders == null)
            {
                throw new Exception("Invalid orders");
            }

            var ordersDto = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            _cache.Set(cacheKey, orders, TimeSpan.FromMinutes(30));
            return ordersDto;
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
                _cache.Remove(OrderCacheKey);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update order status", ex);

            }
        }
    }
}

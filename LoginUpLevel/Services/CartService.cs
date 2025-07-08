using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using System.Net;

namespace LoginUpLevel.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddToCartAsync(CartItemDTO cartItemDto, int customerId)
        {
            try
            {
                var cartItem = _mapper.Map<CartItem>(cartItemDto);
                if (cartItem == null)
                {
                    throw new ArgumentNullException(nameof(cartItemDto), "Cart item cannot be null");
                }
                var product = await _unitOfWork.ProductRepository.GetById(cartItemDto.ProductId);
                var cart = await _unitOfWork.CartRepository.GetCartByCustomerIdAsync(customerId);
                if (cart == null)
                {
                    throw new Exception($"Cart with ID {cart.Id} not found");
                }

                var productItem = await _unitOfWork.CartItemRepository.GetCartItemByProductAndCartAsync(product.Id, cart.Id);

                if(productItem == null)
                {
                    cartItem.TotalPrice = product.Price * cartItemDto.AMount;
                    cartItem.CartId = cart.Id;
                    await _unitOfWork.CartItemRepository.Add(cartItem);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    productItem.AMount += cartItemDto.AMount;
                    productItem.TotalPrice += product.Price * cartItemDto.AMount;
                    await _unitOfWork.CartItemRepository.Update(productItem);
                    await _unitOfWork.SaveChangesAsync();
                }

                cart.TotalPrice += cartItemDto.TotalPrice;
                await _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add item to cart", ex);
            }
        }

        public async Task DeleteCartItemAsync(int productId)
        {
            try
            {
                var cartItem = await _unitOfWork.CartItemRepository.GetById(productId);
                if (cartItem == null)
                {
                    throw new Exception($"Cart item with ID {productId} not found");
                }
                await _unitOfWork.CartItemRepository.Delete(cartItem);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 throw new Exception("Failed to delete cart item", ex);
            }
        }

        public async Task<CartDTO> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByCustomerIdAsync(customerId);
                if (cart == null)
                {
                    throw new Exception($"Cart for customer ID {customerId} not found");
                }
                var cartDto = _mapper.Map<CartDTO>(cart);
                var cartItems = await _unitOfWork.CartItemRepository.GetCartItemByCartAsync(cart.Id);
                if (cartItems == null)
                {
                    throw new Exception($"No items found in cart for customer ID {customerId}");
                }
                var cartItemsDto = _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);

                cartDto.CartItems = cartItemsDto.ToList();
                return cartDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve cart", ex);
            }
        }
        public async Task OrderInCart(OrderDTO orderDto ,int customerId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByCustomerIdAsync(customerId);
                if (cart == null)
                {
                    throw new Exception($"Cart with ID {customerId} not found");
                }

                var cartItems = await _unitOfWork.CartItemRepository.GetCartItemByCartAsync(cart.Id);
                if (cartItems == null)
                {
                    throw new Exception($"No items found in cart with ID {customerId}");
                }

                var orderAddress = await _unitOfWork.OrderAdressRepository.GetById(orderDto.OrderAdressId);
                if (orderAddress == null)
                {
                    throw new Exception($"No address found for customer ID {cart.CustomerId}");
                }

                var order = MapOrderInCartData(orderAddress, customerId);
                order.Note = orderDto.Note;
                await _unitOfWork.OrderRepository.Add(order);
                await _unitOfWork.SaveChangesAsync();

                //Chuyen thong tin cac product trong gio hang sang chi tiet don hang
                foreach (var item in cartItems)
                {
                    var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                    var orderItem = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        AMount = item.AMount,
                        TotalPrice = item.AMount * product.Price
                    };
                    product.AMount -= item.AMount;
                    await _unitOfWork.ProductRepository.Update(product);
                    await _unitOfWork.OrderDetailRepository.Add(orderItem);
                    await _unitOfWork.CartItemRepository.Delete(item);
                    await _unitOfWork.SaveChangesAsync();
                    order.TotalPrice += orderItem.TotalPrice;
                }

                await _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.SaveChangesAsync();
                cart.TotalPrice = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to order in cart", ex);
            }
        }

        public async Task UpdateCartAsync(CartItemDTO cartItemDto)
        {
            try
            {
                var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByProductAsync(cartItemDto.ProductId);
                if (cartItem == null)
                {
                    throw new Exception($"Cart item with Product ID {cartItemDto.ProductId} not found");
                }
                cartItem.AMount = cartItemDto.AMount;

                await _unitOfWork.CartItemRepository.Update(cartItem);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update cart", ex);
            }
        }
        private Order MapOrderInCartData(OrderAdress orderAddress, int customerId)
        {
            return new Order
                {
                Name = orderAddress.Name,
                PhoneNumber = orderAddress.PhoneNumber,
                Email = orderAddress.Email,
                Address = orderAddress.Address,
                CustomerId = customerId,
                TotalPrice = 0
                }
            ;
        }
    }
}

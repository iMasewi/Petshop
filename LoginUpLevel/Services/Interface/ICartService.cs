using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface ICartService
    {
        Task AddToCartAsync(CartItemDTO cartItemDto, int cartId);
        Task<CartDTO> GetCartByCustomerIdAsync(int customerId);
        Task UpdateCartAsync(CartItemDTO cartItemDto);
        Task DeleteCartItemAsync(int productId);
        Task OrderInCart(OrderDTO orderDto, int customerid);
    }
}

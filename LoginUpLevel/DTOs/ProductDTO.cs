using LoginUpLevel.Models;
using static LoginUpLevel.DTOs.UserDTO;

namespace LoginUpLevel.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public float Price { get; set; }
        public int? AMount { get; set; }
        public string? Gender { get; set; } = string.Empty;
        public string? Weight { get; set; } = string.Empty;
        public string? Size { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public List<string>? NameColors { get; set; } = [];
        public string? Image { get; set; }
        public List<EmployeeDTO> Employees { get; } = [];
        public List<OrderDTO> Orders { get; } = [];
        public List<CartDTO> Carts { get; } = [];
        public List<Color> Colors { get; } = [];
        public ICollection<ProductManagerDTO> ProductImages { get; } = new List<ProductManagerDTO>();
        public ICollection<OrderDetailDTO> OrderDetails { get; } = new List<OrderDetailDTO>();
        public ICollection<CartItemDTO> CartItems { get; } = new List<CartItemDTO>();
        public string? ProductColorsJson { get; set; }
        public ICollection<ProductColorDTO> ProductColors { get; set; } = new List<ProductColorDTO>();
    }
}

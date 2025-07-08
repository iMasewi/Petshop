using System.ComponentModel.DataAnnotations.Schema;

namespace LoginUpLevel.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int AMount { get; set; }
        public string? Gender { get; set; } = string.Empty;
        public string? Weight { get; set; } = string.Empty;
        public string? Size { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public List<string>? NameColors { get; set; } = [];
        public string Image { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<Employee> Employee { get; } = [];
        public List<Order> Orders { get; } = [];
        public List<Cart> Carts { get; } = [];
        public List<Color> Colors { get; } = [];
        public ICollection<ProductManager> ProductManager { get; } = new List<ProductManager>();
        public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
        public ICollection<CartItem> CartItem { get; } = new List<CartItem>(); 
        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
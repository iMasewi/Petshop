namespace LoginUpLevel.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public float TotalPrice { get; set; } = 0;
        public List<Product> Products { get; set; } = [];
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

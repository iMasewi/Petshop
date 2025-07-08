namespace LoginUpLevel.Models
{
    public class CartItem
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;   
        public int AMount { get; set; }
        public float TotalPrice { get; set; }
    }
}

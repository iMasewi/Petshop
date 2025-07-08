namespace LoginUpLevel.Models
{
    public class OrderDetail
    {
        public int AMount { get; set; }
        public float TotalPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}

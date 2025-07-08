namespace LoginUpLevel.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? StatusName { get; set; } = "Pending";
        public float? TotalPrice { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CustomerId { get; set; }
        public int? StatusId { get; set; } = 1;
        public Customer Customer { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public List<Product> Products { get; } = [];
        public ICollection<OrderDetail> OrderItems { get; set; } = new List<OrderDetail>();
    }
}

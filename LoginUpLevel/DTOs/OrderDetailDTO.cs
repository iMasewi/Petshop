namespace LoginUpLevel.DTOs
{
    public class OrderDetailDTO
    {
        public int AMount { get; set; }
        public float? TotalPrice { get; set; } 
        public float? Price { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
    }
}

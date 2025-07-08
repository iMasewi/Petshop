namespace LoginUpLevel.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? StatusName { get; set; } = string.Empty;
        public float? TotalPrice { get; set; }
        public string? Note { get; set; }
        public int? CustomerId { get; set; }
        public int? StatusId { get; set; }
        public int OrderAdressId { get; set; }
        public List<ProductDTO> Products { get; } = [];
        public ICollection<OrderDetailDTO> OrderItems { get; set; } = new List<OrderDetailDTO>();
    }
}

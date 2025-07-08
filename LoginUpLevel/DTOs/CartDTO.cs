namespace LoginUpLevel.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}

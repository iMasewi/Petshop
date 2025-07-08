namespace LoginUpLevel.Models
{
    public class OrderAdress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}

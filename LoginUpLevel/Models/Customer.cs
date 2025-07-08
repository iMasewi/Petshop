namespace LoginUpLevel.Models
{
    public class Customer : User
    {
        public List<Order> Orders { get; } = [];
        public ICollection<OrderAdress> OrderAdress { get; } = new List<OrderAdress>();
    }
}

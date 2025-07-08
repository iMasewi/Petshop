namespace LoginUpLevel.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Order> Orders { get; } = [];
    }
}

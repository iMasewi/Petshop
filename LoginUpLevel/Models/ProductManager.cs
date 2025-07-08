namespace LoginUpLevel.Models
{
    public class ProductManager
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}

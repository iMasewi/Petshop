namespace LoginUpLevel.Models
{
    public class Employee : User
    {
        public float Salary { get; set; }
        public List<Product> Products { get; } = [];
        public ICollection<ProductManager> ProductManagers { get; } = new List<ProductManager>();
    }
}

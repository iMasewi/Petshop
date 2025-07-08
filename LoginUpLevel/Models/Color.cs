namespace LoginUpLevel.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string NameColor { get; set; }
        public List<Product> Product { get; } = null!;
        public ICollection<ProductColor> ProductColors { get; } = new List<ProductColor>();
    }
}

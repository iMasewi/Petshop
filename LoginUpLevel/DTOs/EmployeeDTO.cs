namespace LoginUpLevel.DTOs
{
    public class EmployeeDTO : UserDTO
    {
        public float Salary { get; set; }
        public int StaffRole { get; set; }
        public List<ProductDTO> Products { get; } = [];
        public ICollection<ProductManagerDTO> ProductManagers { get; set; } = new List<ProductManagerDTO>();
    }
}

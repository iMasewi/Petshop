namespace LoginUpLevel.DTOs
{
    public class CustomerDTO : UserDTO
    {
        public List<OrderDTO> Orders { get; } = [];
        public ICollection<OrderDTO> OrderDetails { get; } = new List<OrderDTO>();
        public ICollection<OrderAdressDTO> OrderAdress { get; } = new List<OrderAdressDTO>();
        public ICollection<CommentDTO> Comments { get; } = new List<CommentDTO>();
    }
}

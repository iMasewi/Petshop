using System.ComponentModel.DataAnnotations;

namespace LoginUpLevel.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

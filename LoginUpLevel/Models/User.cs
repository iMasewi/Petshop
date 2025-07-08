using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LoginUpLevel.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
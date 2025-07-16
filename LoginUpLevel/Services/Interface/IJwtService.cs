using LoginUpLevel.DTOs;
using LoginUpLevel.Models;

namespace LoginUpLevel.Services.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(UserDTO userDto);
    }
}

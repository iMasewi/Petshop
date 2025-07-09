using LoginUpLevel.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LoginUpLevel.Services
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
    }
}
using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
        string GenerateJwtToken(UserDTO user);
    }
}

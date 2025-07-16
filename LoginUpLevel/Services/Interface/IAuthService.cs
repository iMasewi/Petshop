using LoginUpLevel.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LoginUpLevel.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
        Task<string> LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal);
    }
}
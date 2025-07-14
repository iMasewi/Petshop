using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using LoginUpLevel.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginUpLevel.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(IJwtService jwtService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<UserDTO> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
                if (user == null || result == null)
                {
                    throw new UnauthorizedAccessException("Invalid information");
                }
                var userDto = await MapToUserDto(user);
                var token = await _jwtService.GenerateTokenAsync(user);
                userDto.Token = token;
                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed", ex);
            }
        }
        private async Task<UserDTO> MapToUserDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Logout failed", ex);
            }
        }
    }
}
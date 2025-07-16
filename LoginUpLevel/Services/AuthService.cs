using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories;
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
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IJwtService jwtService, SignInManager<User> signInManager, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> LoginAsync(LoginDTO loginDto)
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
                var token = await _jwtService.GenerateTokenAsync(userDto);
                //userDto.Token = token;
                return token;
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

        public async Task<string> LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal)
        {
            if(claimsPrincipal == null)
            {
                throw new Exception("Google orr ClaimsPrinvipal is null");
            }
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            
            if( email == null)
            {
                throw new Exception("Google");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                var newUser = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                    LastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty
                };

                var userDto = await MapToUserDto(newUser);

                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded)
                {
                    throw new Exception("Unable to create user");
                }

                var userRole = await _userManager.AddToRoleAsync(newUser, "Customer");
                if (!userRole.Succeeded)
                {
                    throw new Exception("Failed to add user to role: ");
                }

                var info = new UserLoginInfo("Google",
                claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                "Google");
                var loginResult = await _userManager.AddLoginAsync(newUser, info);

                if (!loginResult.Succeeded)
                {
                    throw new Exception("Unable to add Google login");
                }
                //Add to cart
                var newCart = new Cart
                {
                    CustomerId = newUser.Id,
                    TotalPrice = 0
                };
                await _unitOfWork.CartRepository.Add(newCart);
                await _unitOfWork.SaveChangesAsync();

                return await _jwtService.GenerateTokenAsync(userDto);
            }
            else
            {
                var userDto = await MapToUserDto(user);
                return await _jwtService.GenerateTokenAsync(userDto);
            }
        }
    }
}
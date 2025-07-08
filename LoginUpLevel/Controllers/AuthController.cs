using LoginUpLevel.DTOs;
using LoginUpLevel.Services;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDto)
        {
            return await _authService.LoginAsync(loginDto);
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}

using LoginUpLevel.DTOs;
using LoginUpLevel.Services;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LoginUpLevel.Models;
using Microsoft.AspNetCore.Routing;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IAuthService authService, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _authService = authService;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);

            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await _authService.LogoutAsync();
        }

        [HttpGet("loginGoogle")]
        public IActionResult LoginGoogle([FromQuery] string returnUrl, LinkGenerator linkGenerator, SignInManager<User> signManager)
        {
            var callbackUrl = linkGenerator.GetPathByName(HttpContext, "GoogleLoginCallback")
                              + $"?returnUrl={returnUrl}";

            var properties = signManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl);

            return Challenge(properties, "Google");
        }


        [HttpGet("loginGoogle/callback", Name = "GoogleLoginCallback")]
        public async Task<IActionResult> GoogleLoginCallback([FromQuery] string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var token = await _authService.LoginWithGoogleAsync(result.Principal);

            return Ok(new {Token = token });
        }
    }
}

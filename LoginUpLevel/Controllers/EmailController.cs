using LoginUpLevel.DTOs;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }
            try
            {
                var otp = await _emailService.GenerateEmailOtpAsync(email);
                return Ok(new { Otp = otp });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] EmailOtpDTO emailOtpDto)
        {
            if (emailOtpDto == null || string.IsNullOrEmpty(emailOtpDto.Otp) || string.IsNullOrEmpty(emailOtpDto.StringEncrypted))
            {
                return BadRequest("Invalid OTP data.");
            }
            try
            {
                var isValid = await _emailService.VerifyEmailOtp(emailOtpDto);
                if (isValid)
                {
                    return Ok("OTP is valid.");
                }
                return BadRequest("Invalid OTP.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailOtpAsync(string to, string subject, EmailViewDTO emailOtpDto);
        Task<string> GenerateEmailOtpAsync(string email);
        Task<bool> VerifyEmailOtp(EmailOtpDTO emailOtpDto);
    }
}

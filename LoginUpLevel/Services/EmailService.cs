using LoginUpLevel.DTOs;
using LoginUpLevel.Services.Interface;
using System.Security.Cryptography;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using Microsoft.AspNetCore.Mvc;

namespace LoginUpLevel.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;
        public EmailService(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _configuration = configuration;
            _viewRenderService = viewRenderService;
        }

        public async Task<string> GenerateEmailOtpAsync(string email)
        {
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();
            var randomStringEncrypted = await Encrypted(otp);
            var emailOtpDto = new EmailViewDTO
            {
                Title = "Email OTP Verification",
                Content = "Đây là thông tin đăng nhập của bạn:",
                ActionContent = otp
            };
            await SendEmailOtpAsync(email, emailOtpDto.Title, emailOtpDto);
            return randomStringEncrypted;
        }

        public async Task SendEmailOtpAsync(string to, string subject, EmailViewDTO emailOtpDto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:Mail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            var body = GetEmailBody(emailOtpDto);
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<bool> VerifyEmailOtp(EmailOtpDTO emailOtpDto)
        {
            var otp = await Encrypted(emailOtpDto.Otp); 
            if (otp.Equals(emailOtpDto.StringEncrypted)) return true;
            return false;
        }
        private async Task<string> Encrypted(string randomString)
        {
            var randomStringEncrypted = "";
            string EncryptionKey = "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(randomString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    randomStringEncrypted = Convert.ToHexString(ms.ToArray());
                }
            }
            return randomStringEncrypted;
        }
        private string GetEmailBody(EmailViewDTO emailViewDTO)
        {
            return $@"
                <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 40px auto; padding: 30px; border: 1px solid #e0e0e0; border-radius: 10px; background-color: #ffffff;"">
                    <h1 style=""color: #1d4ed8; text-align: center; margin-bottom: 20px;"">{emailViewDTO.Title}</h1>

                    <p style=""font-size: 16px; color: #333333; text-align: center; margin-bottom: 20px;"">
                        {emailViewDTO.Content}
                    </p>

                    <div style=""text-align: center; margin: 30px 0;"">
                        <span style=""font-size: 24px; font-weight: bold; letter-spacing: 6px; background-color: #eef6ff; padding: 10px 20px; border-radius: 6px; color: #1d4ed8;"">
                            {emailViewDTO.ActionContent}
                        </span>
                    </div>

                    <p style=""font-size: 13px; color: #999999; text-align: center; margin-top: 40px;"">
                        * Đây là email tự động, vui lòng không phản hồi lại.
                    </p>
                </div>";
        }
    }
}

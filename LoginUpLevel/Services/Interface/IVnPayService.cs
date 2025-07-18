using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrlAsync(PaymentRequestDTO request, HttpContext context);
        Task<PaymentResponseDTO> ProcessPaymentReturnAsync(PaymentReturnDTO vnpayData);
        Task<bool> ValidateSignatureAsync(PaymentReturnDTO vnpayData);
    }
}

//using LoginUpLevel.DTOs;
//using LoginUpLevel.Services.Interface;
//using LoginUpLevel.Types;
//using Microsoft.Extensions.Options;
//using System.Globalization;

//namespace LoginUpLevel.Services
//{
//    public class VnPayService : IVnPayService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<VnPayService> _logger;

//        public VNPayService(IConfiguration configuration, ILogger<VnPayService> logger)
//        {
//            _configuration = configuration;
//            _logger = logger;
//        }

//        public async Task<string> CreatePaymentUrlAsync(PaymentRequestDTO request, HttpContext context)
//        {
//            try
//            {
//                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration);
//                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
//                var tick = DateTime.Now.Ticks.ToString();
//                var pay = new VnPayLibrary();
//                var urlCallBack = _settings.ReturnUrl;

//                pay.AddRequestData("vnp_Version", _settings.Version);
//                pay.AddRequestData("vnp_Command", _settings.Command);
//                pay.AddRequestData("vnp_TmnCode", _settings.TmnCode);
//                pay.AddRequestData("vnp_Amount", ((int)request.TotalPrice * 100).ToString());
//                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
//                pay.AddRequestData("vnp_CurrCode", _settings.CurrCode);
//                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
//                pay.AddRequestData("vnp_Locale", _settings.Locale);
//                pay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang: {request.OrderId}");
//                pay.AddRequestData("vnp_OrderType", request.OrderDescription);
//                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
//                pay.AddRequestData("vnp_TxnRef", tick);

//                var paymentUrl = pay.CreateRequestUrl(_settings.BaseUrl, _settings.HashSecret);

//                _logger.LogInformation("VNPay payment URL created for order: {OrderId}", request.OrderId);
//                return paymentUrl;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating VNPay payment URL");
//                throw;
//            }
//        }

//        public async Task<VNPayResponseDTO> ProcessPaymentReturnAsync(VNPayReturnDTO vnpayData)
//        {
//            try
//            {
//                var response = new VNPayResponseDTO();

//                if (await ValidateSignatureAsync(vnpayData))
//                {
//                    if (vnpayData.vnp_ResponseCode == "00" && vnpayData.vnp_TransactionStatus == "00")
//                    {
//                        response.Success = true;
//                        response.PaymentMethod = "VnPay";
//                        response.OrderDescription = vnpayData.vnp_OrderInfo;
//                        response.OrderId = vnpayData.vnp_TxnRef;
//                        response.TransactionId = vnpayData.vnp_TransactionNo;
//                        response.Token = vnpayData.vnp_TxnRef;
//                        response.VnPayResponseCode = vnpayData.vnp_ResponseCode;
//                        response.Amount = Convert.ToDecimal(vnpayData.vnp_Amount) / 100;
//                        response.PaymentDate = DateTime.ParseExact(vnpayData.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
//                        response.ResponseMessage = "Giao dịch thành công";
//                    }
//                    else
//                    {
//                        response.Success = false;
//                        response.ResponseMessage = "Giao dịch thất bại";
//                    }
//                }
//                else
//                {
//                    response.Success = false;
//                    response.ResponseMessage = "Chữ ký không hợp lệ";
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error processing VNPay return");
//                throw;
//            }
//        }

//        public async Task<bool> ValidateSignatureAsync(VNPayReturnDTO vnpayData)
//        {
//            try
//            {
//                var pay = new VnPayLibrary();
//                var inputHash = vnpayData.vnp_SecureHash;

//                // Tạo lại hash từ dữ liệu trả về
//                var rspraw = GetResponseData(vnpayData);
//                var myChecksum = pay.CreateResponseData(rspraw, _settings.HashSecret);

//                return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error validating VNPay signature");
//                return false;
//            }
//        }

//        private SortedList<string, string> GetResponseData(VNPayReturnDTO vnpayData)
//        {
//            var vnpayProperties = typeof(VNPayReturnDTO).GetProperties();
//            var responseData = new SortedList<string, string>();

//            foreach (var property in vnpayProperties)
//            {
//                if (property.Name != "vnp_SecureHash" && property.Name != "vnp_SecureHashType")
//                {
//                    var value = property.GetValue(vnpayData)?.ToString();
//                    if (!string.IsNullOrEmpty(value))
//                    {
//                        responseData.Add(property.Name, value);
//                    }
//                }
//            }

//            return responseData;
//        }
//    }
//}

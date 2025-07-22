using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OGCCinema.Services
{
    public class PayOSService
    {
        private readonly PayOS _payOS;

        public PayOSService(IConfiguration configuration)
        {
            string clientId = configuration["PayOS:ClientId"] ?? throw new Exception("Cannot find Client ID");
            string apiKey = configuration["PayOS:ApiKey"] ?? throw new Exception("Cannot find API Key");
            string checksumKey = configuration["PayOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key");
            _payOS = new PayOS(clientId, apiKey, checksumKey);
        }

        public async Task<string> CreatePaymentAsync(PayOSCreatePaymentRequest request)
        {
            try
            {
                string description = (request.Description ?? "Thanh toan").Substring(0, Math.Min(25, (request.Description ?? "Thanh toan").Length));
                int orderCode = request.OrderCode == 0 ? (int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() % int.MaxValue) : request.OrderCode;
                int expiredAt = request.ExpiredAt == 0 ? (int)(DateTimeOffset.Now.AddMinutes(30).ToUnixTimeSeconds()) : request.ExpiredAt;

                var items = request.Items?.Select(i => new ItemData(i.Name, i.Quantity, (int)i.Price)).ToList() ??
                            new List<ItemData> { new ItemData(description, 1, (int)request.Amount) };

                var paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: (int)request.Amount,
                    description: description,
                    items: items,
                    cancelUrl: request.CancelUrl,
                    returnUrl: request.ReturnUrl,
                    expiredAt: expiredAt
                );

                CreatePaymentResult result = await _payOS.createPaymentLink(paymentData);
                return result.checkoutUrl ?? throw new Exception("PAYOS không trả về checkoutUrl");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo thanh toán: {ex.Message}", ex);
            }
        }
    }

    public class PayOSCreatePaymentRequest
    {
        public int OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public List<PaymentItem> Items { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public int ExpiredAt { get; set; }
    }

    public class PaymentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OGCCinema.Data;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OGCCinema.Services;
using OGCCinema.Models;
using QRCoder;
using System.Drawing; 
using System.Drawing.Imaging;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace OGCCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu đăng nhập
    public class MonAnController : Controller
    {
        private readonly OgccinemaContext _context;
        private readonly PayOSService _payOSService;

        public MonAnController(OgccinemaContext context, PayOSService payOSService)
        {
            _context = context;
            _payOSService = payOSService;
        }

        [HttpGet("/MonAn")]
        public async Task<IActionResult> Index()
        {
            var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated) || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            var monAns = await _context.Monans
                .Include(m => m.IdloaiMonAnNavigation)
                .OrderBy(m => m.IdloaiMonAnNavigation.TenLoai)
                .ToListAsync();
            return View(monAns);
        }

        [HttpGet("/MonAn/Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpGet("/MonAn/Payment")]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpGet("/MonAn/UpdateEmail")]
        public IActionResult UpdateEmail()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            return View(model: email);
        }

        [HttpPost("/MonAn/UpdateEmail")]
        public IActionResult UpdateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email không được để trống.");
                return View();
            }
            HttpContext.Session.SetString("UserEmail", email);
            return RedirectToAction("Checkout");
        }

        [HttpGet("/MonAn/PaymentQR")]
        public IActionResult PaymentQR()
        {
            var paymentUrl = TempData["PaymentUrl"]?.ToString();
            if (string.IsNullOrEmpty(paymentUrl))
            {
                TempData["Error"] = "Không thể tạo mã QR thanh toán.";
                return RedirectToAction("Checkout");
            }
            return View(new { PaymentUrl = paymentUrl });
        }

        [HttpGet("/MonAn/PaymentSuccess")]
        public async Task<IActionResult> PaymentSuccess(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                return RedirectToAction("Checkout");
            }

            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Vui lòng cập nhật email trong phần Tài khoản.";
                return RedirectToAction("UpdateEmail");
            }

            var cart = System.Text.Json.JsonSerializer.Deserialize<List<PaymentItem>>(TempData.Peek("Cart")?.ToString() ?? "[]");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Checkout");
            }

            try
            {
                // Lấy username từ session
                var username = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("Không tìm thấy thông tin người dùng.");
                }

                // Lấy thông tin khách hàng từ bảng Khachhang
                var khachHang = await _context.Khachhangs
                    .FirstOrDefaultAsync(k => k.Username == username);
                if (khachHang == null)
                {
                    throw new Exception("Không tìm thấy thông tin khách hàng.");
                }
                string sdt = khachHang.Sdt ?? ""; 
                string tenKhachHang = khachHang.HoTen ?? ""; 

                // Lưu hóa đơn vào HD_MONAN
                var hdMonan = new HdMonan
                {
                    NgayMua = DateTime.Now,
                    TongTien = cart.Sum(item => item.Price * item.Quantity),
                    IdkhachHang = khachHang.Id,
                    SdtKh = sdt,
                    TenKh = tenKhachHang,
                };
                _context.HdMonans.Add(hdMonan);
                await _context.SaveChangesAsync();

                // Lưu chi tiết hóa đơn vào CTHD_MONAN
                foreach (var item in cart)
                {
                    var cthd = new CthdMonan
                    {
                        IdhoaDon = hdMonan.Id,
                        IdmonAn = int.TryParse(item.Id, out int id) ? id : (int?)null,
                        TenMonAn = item.Name,
                        Gia = item.Price,
                        SoLuong = item.Quantity,
                        NgayMua = DateTime.Now,
                        TrangThai = null,
                    };
                    _context.CthdMonans.Add(cthd);
                }
                await _context.SaveChangesAsync();

                // Lấy dữ liệu từ CTHD_MONAN để hiển thị
                var cthdList = await _context.CthdMonans
                    .Where(c => c.IdhoaDon == hdMonan.Id)
                    .ToListAsync();
                ViewBag.OrderCode = orderCode;
                ViewBag.Items = cthdList;
                ViewBag.TotalAmount = cthdList.Sum(c => c.Gia * c.SoLuong);

                // Tạo mã QR từ Id của HD_MONAN
                string qrCodeText = $"{hdMonan.Id}"; 
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
                    using (var qrCode = new PngByteQRCode(qrCodeData)) // Sử dụng PngByteQRCode
                    {
                        var qrCodeBytes = qrCode.GetGraphic(20); // Lấy byte array
                        var base64String = Convert.ToBase64String(qrCodeBytes);
                        ViewBag.QRCodeImage = $"data:image/png;base64,{base64String}";
                    }
                }

                // Gửi email, sử dụng ViewBag.QRCodeImage đã tạo
                await SendEmailAsync(email, hdMonan.Id, cthdList, ViewBag.QRCodeImage);

                // Xóa giỏ hàng tạm
                TempData.Remove("Cart");
                return View();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.Error = $"Lỗi khi lưu dữ liệu: {errorMessage}";

                ViewBag.OrderCode = orderCode;
                ViewBag.Items = null;
                ViewBag.TotalAmount = 0;

                return View();
            }
        }

        [HttpPost("payment/create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Vui lòng cập nhật email trong phần Tài khoản trước khi thanh toán.");
            }

            if (request == null || request.Amount <= 0 || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            foreach (var item in request.Items)
            {
                if (string.IsNullOrEmpty(item.Name) || item.Price <= 0 || item.Quantity <= 0)
                {
                    return BadRequest($"Item không hợp lệ: {System.Text.Json.JsonSerializer.Serialize(item)}");
                }
            }

            var payOSRequest = new PayOSCreatePaymentRequest
            {
                Amount = request.Amount,
                OrderCode = 0,
                Description = request.Description ?? "Thanh toán đơn hàng",
                Items = request.Items.Select(item => new OGCCinema.Services.PaymentItem // Sử dụng namespace Services
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList(),
                ReturnUrl = "https://localhost:7060/MonAn/PaymentSuccess?orderCode={orderCode}",
                CancelUrl = "https://localhost:7060/MonAn/Checkout",
                ExpiredAt = 0
            };

            try
            {
                var paymentUrl = await _payOSService.CreatePaymentAsync(payOSRequest);
                Console.WriteLine($"PAYOS Response - Payment URL: {paymentUrl}");
                TempData["PaymentUrl"] = paymentUrl;
                TempData["Cart"] = System.Text.Json.JsonSerializer.Serialize(request.Items);
                return Ok(new { paymentUrl = paymentUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception details: {ex.Message} - StackTrace: {ex.StackTrace}");
                return BadRequest($"Lỗi khi gọi PAYOS API: {ex.Message}");
            }
        }

        private async Task SendEmailAsync(string email, int orderId, List<CthdMonan> items, string qrCodeImage)
        {
            var mail = new MailMessage
            {
                From = new MailAddress("nhoangnghia2104@gmail.com"),
                Subject = $"Hóa đơn thanh toán - Đơn hàng #{orderId}",
                IsBodyHtml = true
            };
            mail.To.Add(email);

            // Tách phần base64 từ chuỗi "data:image/png;base64,..."
            var base64Data = qrCodeImage.Substring(qrCodeImage.IndexOf(",") + 1);
            var qrBytes = Convert.FromBase64String(base64Data);
            var stream = new System.IO.MemoryStream(qrBytes);

            // Tạo ảnh nhúng qua ContentId
            var linkedResource = new LinkedResource(stream, "image/png")
            {
                ContentId = "qrCodeImage",
                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };

            // Soạn nội dung email
            string htmlBody = $@"
                <h2>Hóa đơn thanh toán</h2>
                <p>Cảm ơn bạn đã mua hàng tại OGC Cinema!</p>
                <p><strong>Mã đơn hàng:</strong> {orderId}</p>
                <p><strong>Ngày mua:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                <h3>Chi tiết đơn hàng:</h3>
                <ul>
                    {string.Join("", items.Select(item => $"<li>{item.TenMonAn} - SL: {item.SoLuong} - Giá: {(item.Gia * item.SoLuong):N0}đ</li>"))}
                </ul>
                <p><strong>Tổng cộng:</strong> {items.Sum(i => i.Gia * i.SoLuong):N0}đ</p>
                <h3>Mã QR:</h3>
                <img src='cid:qrCodeImage' style='max-width:200px; max-height:200px;' />
            ";

            var alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
            alternateView.LinkedResources.Add(linkedResource);
            mail.AlternateViews.Add(alternateView);

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new System.Net.NetworkCredential("nhoangnghia2104@gmail.com", "arxw alfv jwxf vvwe");
                smtp.EnableSsl = true;
                await Task.Run(() => smtp.Send(mail));
            }
        }


    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public List<PaymentItem> Items { get; set; }
        public string Method { get; set; }
    }

    public class PaymentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
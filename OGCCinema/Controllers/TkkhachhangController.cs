//using Microsoft.AspNetCore.Mvc;
//using OGCCinema.Data;
//using OGCCinema.Models.ViewModels;
//using OGCCinema.Models;
//using System.Text;
//using System.Security.Cryptography;
//using Microsoft.AspNetCore.Http;
//using System.Net.Mail;
//using System.Net;
//using Microsoft.AspNetCore.Authorization;
//using System.Text.RegularExpressions;
//using Microsoft.EntityFrameworkCore;

//namespace OGCCinema.Controllers
//{
//    public class TkkhachhangController : Controller
//    {
//        private readonly OgccinemaContext _context;

//        public TkkhachhangController(OgccinemaContext context)
//        {
//            _context = context;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public IActionResult Register() => View();


//        private byte[] HashPassword(string password)
//        {
//            using var sha256 = SHA256.Create();
//            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//        }

//        private bool VerifyPassword(string inputPassword, byte[] storedHash)
//        {
//            var inputHash = HashPassword(inputPassword);
//            return inputHash.SequenceEqual(storedHash);
//        }

//        //----------hàm đăg ký tài khoản
//        [HttpPost]
//        public JsonResult RegisterAjax([FromBody] RegisterViewModel model)
//        {
//            if (_context.Tkkhachhangs.Any(t => t.Username == model.Username))
//            {
//                return Json(new { success = false, message = "Tài khoản đã tồn tại" });
//            }

//            var hashedPassword = HashPassword(model.Password);

//            var tk = new Tkkhachhang
//            {
//                Username = model.Username,
//                Password = hashedPassword,
//                NgayTao = DateTime.Now
//            };

//            var kh = new Khachhang
//            {
//                Username = model.Username
//            };

//            _context.Tkkhachhangs.Add(tk);
//            _context.Khachhangs.Add(kh);
//            _context.SaveChanges();

//            return Json(new { success = true, message = "Đăng ký thành công" });
//        }

//        //----------hàm đăng nhập
//        [HttpPost]
//        public JsonResult LoginAjax([FromBody] LoginViewModel model)
//        {
//            var user = _context.Tkkhachhangs.FirstOrDefault(u => u.Username == model.Username);
//            if (user == null || !VerifyPassword(model.Password, user.Password))
//            {
//                return Json(new { success = false, message = "Sai tài khoản hoặc mật khẩu" });
//            }

//            // Lưu vào session
//            HttpContext.Session.SetString("username", user.Username);
//            return Json(new { success = true, message = "Đăng nhập thành công" });
//        }

//        //----------quên mật khẩu
//        [AllowAnonymous] //truy cập mà không cần mật khẩu -> cần để quên mật khẩu
//                         // GET: QuenMatKhau
//        public async Task<IActionResult> QuenMatKhau(string username)
//        {
//            if (string.IsNullOrEmpty(username))
//            {
//                TempData["Error"] = "Vui lòng nhập tên đăng nhập.";
//                return RedirectToAction("DangNhap", "Home");
//            }

//            var user = await _context.Tkkhachhangs.FirstOrDefaultAsync(t => t.Username == username);
//            if (user == null)
//            {
//                TempData["Error"] = "Không tồn tại tài khoản này.";
//                return RedirectToAction("DangNhap", "Home");
//            }

//            var khachHang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Username == username);
//            if (khachHang == null || string.IsNullOrEmpty(khachHang.Email))
//            {
//                TempData["Error"] = "Quý khách chưa cập nhật email.";
//                return RedirectToAction("DangNhap", "Home");
//            }

//            // Tạo mật khẩu mới tạm
//            string newPassword = GenerateRandomPassword(8); 
//            user.Password = HashPassword(newPassword); 
//            await _context.SaveChangesAsync();

//            await SendEmailAsync(khachHang.Email, "Khôi phục mật khẩu", $"Mật khẩu mới của bạn là: {newPassword}");

//            // Điều hướng đến trang đổi mật khẩu
//            TempData["username"] = username;
//            return RedirectToAction("DoiMatKhauTuEmail");
//        }

//        public IActionResult DoiMatKhauTuEmail()
//        {
//            var username = TempData["username"]?.ToString();
//            if (string.IsNullOrEmpty(username)) return RedirectToAction("DangNhap", "Home");

//            ViewBag.Username = username;
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> DoiMatKhauTuEmail(string username, string oldPassword, string newPassword, string confirmPassword)
//        {
//            var user = await _context.Tkkhachhangs.FirstOrDefaultAsync(t => t.Username == username);
//            if (user == null || !VerifyPassword(oldPassword, user.Password))
//            {
//                ViewBag.Error = "Mật khẩu hiện tại không đúng.";
//                ViewBag.Username = username;
//                return View();
//            }

//            if (newPassword != confirmPassword)
//            {
//                ViewBag.Error = "Xác nhận mật khẩu không khớp.";
//                ViewBag.Username = username;
//                return View();
//            }

//            if (!Regex.IsMatch(newPassword, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
//            {
//                ViewBag.Error = "Mật khẩu mới phải từ 8 ký tự và bao gồm chữ, số và ký tự đặc biệt.";
//                ViewBag.Username = username;
//                return View();
//            }

//            user.Password = HashPassword(newPassword);
//            await _context.SaveChangesAsync();

//            TempData["Success"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập.";
//            return RedirectToAction("DangNhap", "Home");
//        }


//        //----------hàm tạo mật khẩu tự động
//        private string GenerateRandomPassword(int length)
//        {
//            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
//            var random = new Random();
//            return new string(Enumerable.Repeat(chars, length)
//                .Select(s => s[random.Next(s.Length)]).ToArray());
//        }

//        //----------hàm gửi mail
//        public async Task SendEmailAsync(string toEmail, string subject, string body)
//        {
//            var fromEmail = "nhoangnghia2104@gmail.com";
//            var fromPassword = "arxw alfv jwxf vvwe";

//            var smtp = new SmtpClient
//            {
//                Host = "smtp.gmail.com",
//                Port = 587,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromEmail, fromPassword)
//            };

//            var message = new MailMessage(fromEmail, toEmail, subject, body);
//            await smtp.SendMailAsync(message);
//        }



//    }
//}

using Microsoft.AspNetCore.Mvc;
using OGCCinema.Data;
using OGCCinema.Models.ViewModels;
using OGCCinema.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace OGCCinema.Controllers
{
    public class TkkhachhangController : Controller
    {
        private readonly OgccinemaContext _context;

        public TkkhachhangController(OgccinemaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register() => View();


        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string inputPassword, byte[] storedHash)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash.SequenceEqual(storedHash);
        }

        //----------hàm đăg ký tài khoản
        [HttpPost]
        public JsonResult RegisterAjax([FromBody] RegisterViewModel model)
        {
            if (_context.Tkkhachhangs.Any(t => t.Username == model.Username))
            {
                return Json(new { success = false, message = "Tài khoản đã tồn tại" });
            }

            var hashedPassword = HashPassword(model.Password);

            var tk = new Tkkhachhang
            {
                Username = model.Username,
                Password = hashedPassword,
                NgayTao = DateTime.Now
            };

            var kh = new Khachhang
            {
                Username = model.Username
            };

            _context.Tkkhachhangs.Add(tk);
            _context.Khachhangs.Add(kh);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đăng ký thành công" });
        }

        //----------hàm đăng nhập
        [HttpPost]
        public async Task<JsonResult> LoginAjax([FromBody] LoginViewModel model)
        {
            var user = _context.Tkkhachhangs.FirstOrDefault(u => u.Username == model.Username);
            if (user == null || !VerifyPassword(model.Password, user.Password))
            {
                return Json(new { success = false, message = "Sai tài khoản hoặc mật khẩu" });
            }

            // Lưu vào session
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("UserAuthenticated", "true");
            HttpContext.Session.SetString("UserEmail", _context.Khachhangs
                .Where(k => k.Username == user.Username)
                .Select(k => k.Email)
                .FirstOrDefault() ?? ""); // Lấy email từ Khachhang

            // Tạo cookie authentication
            var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);

            return Json(new { success = true, message = "Đăng nhập thành công" });
        }

        //----------quên mật khẩu
        [AllowAnonymous] //truy cập mà không cần mật khẩu -> cần để quên mật khẩu
                         // GET: QuenMatKhau
        public async Task<IActionResult> QuenMatKhau(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                TempData["Error"] = "Vui lòng nhập tên đăng nhập.";
                return RedirectToAction("DangNhap", "Home");
            }

            var user = await _context.Tkkhachhangs.FirstOrDefaultAsync(t => t.Username == username);
            if (user == null)
            {
                TempData["Error"] = "Không tồn tại tài khoản này.";
                return RedirectToAction("DangNhap", "Home");
            }

            var khachHang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Username == username);
            if (khachHang == null || string.IsNullOrEmpty(khachHang.Email))
            {
                TempData["Error"] = "Quý khách chưa cập nhật email.";
                return RedirectToAction("DangNhap", "Home");
            }

            // Tạo mật khẩu mới tạm
            string newPassword = GenerateRandomPassword(8);
            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();

            await SendEmailAsync(khachHang.Email, "Khôi phục mật khẩu", $"Mật khẩu mới của bạn là: {newPassword}");

            // Điều hướng đến trang đổi mật khẩu
            TempData["username"] = username;
            return RedirectToAction("DoiMatKhauTuEmail");
        }

        public IActionResult DoiMatKhauTuEmail()
        {
            var username = TempData["username"]?.ToString();
            if (string.IsNullOrEmpty(username)) return RedirectToAction("DangNhap", "Home");

            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoiMatKhauTuEmail(string username, string oldPassword, string newPassword, string confirmPassword)
        {
            var user = await _context.Tkkhachhangs.FirstOrDefaultAsync(t => t.Username == username);
            if (user == null || !VerifyPassword(oldPassword, user.Password))
            {
                ViewBag.Error = "Mật khẩu hiện tại không đúng.";
                ViewBag.Username = username;
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Xác nhận mật khẩu không khớp.";
                ViewBag.Username = username;
                return View();
            }

            if (!Regex.IsMatch(newPassword, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
            {
                ViewBag.Error = "Mật khẩu mới phải từ 8 ký tự và bao gồm chữ, số và ký tự đặc biệt.";
                ViewBag.Username = username;
                return View();
            }

            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap", "Home");
        }


        //----------hàm tạo mật khẩu tự động
        private string GenerateRandomPassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //----------hàm gửi mail
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = "nhoangnghia2104@gmail.com";
            var fromPassword = "arxw alfv jwxf vvwe";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var message = new MailMessage(fromEmail, toEmail, subject, body);
            await smtp.SendMailAsync(message);
        }



    }
}


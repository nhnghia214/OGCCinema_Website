//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using OGCCinema.Models;

//namespace OGCCinema.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult TrangChu()
//        {
//            return View();
//        }
//        public IActionResult DangNhap()
//        {
//            return View();
//        }
//        public IActionResult DangXuat()
//        {
//            // Xóa tất cả session
//            HttpContext.Session.Clear();

//            // Hoặc chỉ xóa từng phần:
//            // HttpContext.Session.Remove("KhachHang");

//            // Điều hướng về trang đăng nhập
//            return RedirectToAction("DangNhap", "Home");
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}


using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OGCCinema.Models;

namespace OGCCinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult TrangChu()
        {
            return View();
        }
        public IActionResult DangNhap()
        {
            return View();
        }
        public IActionResult DangXuat()
        {
            // Xóa cookie authentication
            HttpContext.SignOutAsync();

            // Xóa tất cả session
            HttpContext.Session.Clear();

            // Điều hướng về trang đăng nhập
            return RedirectToAction("DangNhap", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


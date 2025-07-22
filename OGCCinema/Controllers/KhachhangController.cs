using Microsoft.AspNetCore.Mvc;
using OGCCinema.Data;
using OGCCinema.Models;

namespace OGCCinema.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly OgccinemaContext _context;

        public KhachhangController(OgccinemaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("DangNhap", "Home");

            var khach = _context.Khachhangs.FirstOrDefault(k => k.Username == username);
            if (khach == null) return NotFound();

            return View(khach); // truyền model Khachhang sang View
        }

        [HttpPost]
        public IActionResult Update(Khachhang updated)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("DangNhap", "Home");

            var khach = _context.Khachhangs.FirstOrDefault(k => k.Username == username);
            if (khach == null) return NotFound();

            // Cập nhật dữ liệu
            khach.HoTen = updated.HoTen;
            khach.Tuoi = updated.Tuoi;
            khach.DiaChi = updated.DiaChi;
            khach.Sdt = updated.Sdt;
            khach.Email = updated.Email;

            _context.SaveChanges();

            ViewBag.ThongBao = "Cập nhật thành công!";
            return View("Index", khach);
        }
    }
}

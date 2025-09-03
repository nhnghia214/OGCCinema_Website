using Microsoft.AspNetCore.Mvc;
using OGCCinema.Models_Old;
using OGCCinema.Data;

namespace OGCCinema.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly OgccinemaContext _context;

        public ReviewsController(OgccinemaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new ReviewViewModel
            {
                CaLams = _context.CaLams.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRapReview(int IdKhachHang, string NoiDung)
        {
            var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated) || !User.Identity.IsAuthenticated)
            {
                TempData["PendingReview"] = System.Text.Json.JsonSerializer.Serialize(new { IdKhachHang, NoiDung });
                return RedirectToAction("DangNhap", "Home");
            }

            var khachHangId = HttpContext.Session.GetInt32("IdKhachHang") ?? 0;
            if (khachHangId == 0) return Json(new { success = false, message = "Không thể xác định khách hàng." });

            if (string.IsNullOrWhiteSpace(NoiDung)) return Json(new { success = false, message = "Vui lòng nhập nội dung." });

            var review = new DanhGiaRap { IdKhachHang = khachHangId, NoiDung = NoiDung };
            _context.DanhGiaRaps.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("ThankYou");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitNhanVienReview(int IdKhachHang, int IdCa, DateTime NgayLam, string TenNhanVien, string NoiDung)
        {
            var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated) || !User.Identity.IsAuthenticated)
            {
                TempData["PendingReview"] = System.Text.Json.JsonSerializer.Serialize(new { IdKhachHang, IdCa, NgayLam, TenNhanVien, NoiDung });
                return RedirectToAction("DangNhap", "Home");
            }

            var khachHangId = HttpContext.Session.GetInt32("IdKhachHang") ?? 0;
            if (khachHangId == 0) return Json(new { success = false, message = "Không thể xác định khách hàng." });

            if (IdCa == 0 || string.IsNullOrWhiteSpace(NoiDung)) return Json(new { success = false, message = "Vui lòng chọn ca làm và nhập nội dung." });

            var review = new DanhGiaNhanVien
            {
                IdKhachHang = khachHangId,
                IdCa = IdCa,
                NgayLam = NgayLam,
                TenNhanVien = TenNhanVien,
                NoiDung = NoiDung
            };
            _context.DanhGiaNhanViens.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OGCCinema.Data;
using OGCCinema.Models;
using OGCCinema.Models_Old;
using System.Text.Json;

namespace OGCCinema.Controllers
{
    public class PhimController : Controller
    {
        private readonly OgccinemaContext _context;

        public PhimController(OgccinemaContext context)
        {
            _context = context;
        }

        // Hiển thị tất cả phim
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Phims.ToListAsync(); 
            return View(movies);
        }

        // Hiển thị phim đang chiếu
        public async Task<IActionResult> PhimDangChieu()
        {
            var currentDate = DateTime.Today;
            var movies = await _context.Phims
                .Where(p => p.TrangThai == 1 && p.NgayKhoiChieu <= currentDate)
                .ToListAsync();
            return View("Index", movies);
        }

        // Hiển thị phim sắp chiếu
        public async Task<IActionResult> PhimSapChieu()
        {
            var currentDate = DateTime.Today;
            var movies = await _context.Phims
                .Where(p => p.NgayKhoiChieu > currentDate)
                .ToListAsync();
            return View("Index", movies);
        }

        // Controllers/PhimController.cs
        public async Task<IActionResult> Details(int id)
        {
            var phim = await _context.Phims
                .Include(p => p.TheLoaiPhim)
                .Include(p => p.DinhDangPhim)
                .Include(p => p.DoTuoi)
                .Include(p => p.Danhgiaphims)
                    .ThenInclude(d => d.KhachHang) // Lấy thông tin khách hàng
                .Include(p => p.Danhgiaphims)
                    .ThenInclude(d => d.Replies) // Lấy bình luận trả lời
                    .ThenInclude(r => r.KhachHang) // Lấy thông tin khách hàng cho bình luận trả lời
                .FirstOrDefaultAsync(p => p.Id == id);
            if (phim == null) return NotFound();

            // Nếu có đánh giá tạm lưu trước đó
            if (TempData["PendingReview"] != null)
            {
                try
                {
                    var pending = JsonSerializer.Deserialize<PendingReviewModel>(TempData["PendingReview"].ToString());
                    ViewBag.PendingReview = pending;
                }
                catch (JsonException)
                {
                    ViewBag.PendingReview = null;
                }
            }

            return View(phim);
        }

        [HttpPost]
        public ActionResult DanhGia(Danhgiaphim model)
        {
            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
                // Lưu vào DB, ví dụ:
                _context.Danhgiaphims.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = model.Idphim });
            }

            // Nếu lỗi thì quay lại trang details
            return RedirectToAction("Details", new { id = model.Idphim });
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int PhimID, int Rating, string NoiDung)
        {
            // Kiểm tra login
            var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated) || !User.Identity.IsAuthenticated)
            {
                // Lưu tạm đánh giá vào TempData để khi login xong vẫn giữ lại
                TempData["PendingReview"] = JsonSerializer.Serialize(new
                {
                    PhimID,
                    Rating,
                    NoiDung = NoiDung ?? ""
                });

                return RedirectToAction("DangNhap", "Home");
            }

            // Lấy username từ session
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("DangNhap", "Home");
            }

            // Tìm khách hàng theo username
            var khachHang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Username == username);
            if (khachHang == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            // Kiểm tra ít nhất một trong hai trường có giá trị
            if (Rating == 0 && string.IsNullOrWhiteSpace(NoiDung))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập nội dung hoặc chọn số sao để đánh giá.";
                return RedirectToAction("Details", new { id = PhimID });
            }

            // Lưu đánh giá
            var review = new Danhgiaphim
            {
                Idphim = PhimID,
                IdkhachHang = khachHang.Id,
                DiemDanhGia = Rating > 0 ? Rating : 0, // Nếu Rating = 0, lưu null
                NoiDung = string.IsNullOrWhiteSpace(NoiDung) ? null : NoiDung, // Nếu NoiDung rỗng, lưu null
                NgayTao = DateTime.Now
            };

            _context.Danhgiaphims.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = PhimID });
        }


        [HttpPost]
        public async Task<IActionResult> ReplyReview(int PhimID, int ParentID, string NoiDung)
        {
            var isAuthenticated = HttpContext.Session.GetString("UserAuthenticated");
            if (string.IsNullOrEmpty(isAuthenticated) || !User.Identity.IsAuthenticated)
            {
                TempData["PendingReview"] = JsonSerializer.Serialize(new PendingReviewModel
                {
                    PhimID = PhimID,
                    Rating = 0,
                    NoiDung = NoiDung ?? "",
                    ParentID = ParentID
                });
                return RedirectToAction("DangNhap", "Home");
            }

            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("DangNhap", "Home");
            }

            var khachHang = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Username == username);
            if (khachHang == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            if (string.IsNullOrWhiteSpace(NoiDung))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập nội dung trả lời.";
                return RedirectToAction("Details", new { id = PhimID });
            }


            // Kiểm tra xem ParentID có tồn tại không
            var parentReview = await _context.Danhgiaphims.FindAsync(ParentID);
            if (parentReview == null)
            {
                TempData["ErrorMessage"] = "Bình luận gốc không tồn tại.";
                return RedirectToAction("Details", new { id = PhimID });
            }

            var reply = new Danhgiaphim
            {
                Idphim = PhimID,
                IdkhachHang = khachHang.Id,
                NoiDung = NoiDung,
                ParentId = ParentID,
                NgayTao = DateTime.Now
            };

            try
            {
                _context.Danhgiaphims.Add(reply);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Có lỗi khi lưu trả lời. Vui lòng thử lại.";
                return RedirectToAction("Details", new { id = PhimID });
            }

            return RedirectToAction("Details", new { id = PhimID });
        }





    }
}

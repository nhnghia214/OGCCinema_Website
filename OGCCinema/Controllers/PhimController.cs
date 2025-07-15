using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OGCCinema.Data;
using OGCCinema.Models;

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
                .FirstOrDefaultAsync(p => p.Id == id);
            if (phim == null) return NotFound();
            return View(phim);
        }

    }
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Phim
{
    public int Id { get; set; }

    public string TenPhim { get; set; } = null!;

    public string? DaoDien { get; set; }

    public string? DienVien { get; set; }

    public int IdtheLoaiPhim { get; set; }

    public int IddinhDang { get; set; }

    public string? IddoTuoi { get; set; }

    public int ThoiLuong { get; set; }

    public string? MoTa { get; set; }

    public DateTime NgayKhoiChieu { get; set; }

    public int? TrangThai { get; set; }

    public string? TrailerUrl { get; set; }

    public string? PosterUrl { get; set; }

    public string? Anh { get; set; }

   

    // Optional: Navigation properties for related tables
    public Theloaiphim TheLoaiPhim { get; set; }
    public Dinhdangphim DinhDangPhim { get; set; }
    public Dotuoi DoTuoi { get; set; }
    public virtual ICollection<Lichchieu> Lichchieus { get; set; } = new List<Lichchieu>();
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class HdVe
{
    public int Id { get; set; }

    public string? MaKm { get; set; }

    public int? IdnhanVien { get; set; }

    public string? TenNv { get; set; }

    public string? SdtNv { get; set; }

    public int? IdkhachHang { get; set; }

    public string? TenKh { get; set; }

    public string? SdtKh { get; set; }

    public DateTime NgayDat { get; set; }

    public decimal? TongTien { get; set; }

    public virtual Khachhang? IdkhachHangNavigation { get; set; }

    public virtual Nhanvien? IdnhanVienNavigation { get; set; }

    public virtual Khuyenmai? MaKmNavigation { get; set; }

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models;

public partial class HdMonan
{
    [Key]
    public int Id { get; set; }
    [StringLength(20)]
    public string? MaKm { get; set; }

    public int? IdnhanVien { get; set; }
    [StringLength(50)]
    public string? TenNv { get; set; }
    [StringLength(10)]
    public string? SdtNv { get; set; }

    public int? IdkhachHang { get; set; }
    [StringLength(50)]
    public string? TenKh { get; set; }
    [StringLength(10)]
    public string? SdtKh { get; set; }

    public DateTime NgayMua { get; set; } = DateTime.Now;
    [Column(TypeName = "decimal(10,0)")]
    public decimal? TongTien { get; set; }

    public virtual ICollection<CthdMonan> CthdMonans { get; set; } = new List<CthdMonan>();

    public virtual Khachhang? IdkhachHangNavigation { get; set; }

    public virtual Nhanvien? IdnhanVienNavigation { get; set; }

    public virtual Khuyenmai? MaKmNavigation { get; set; }
}

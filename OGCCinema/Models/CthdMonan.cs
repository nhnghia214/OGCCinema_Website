using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class CthdMonan
{
    public int Id { get; set; }

    public string? TenMonAn { get; set; }

    public decimal? Gia { get; set; }

    public int SoLuong { get; set; }

    public DateTime? NgayMua { get; set; }

    public int IdhoaDon { get; set; }

    public int? IdmonAn { get; set; }

    public string? TrangThai { get; set; }

    public virtual HdMonan IdhoaDonNavigation { get; set; } = null!;

    public virtual Monan? IdmonAnNavigation { get; set; }
}

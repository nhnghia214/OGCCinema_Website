using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Khuyenmai
{
    public string MaKm { get; set; } = null!;

    public string? TenKm { get; set; }

    public int? MucGiam { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public string? MoTa { get; set; }

    public string? LoaiApDung { get; set; }

    public virtual ICollection<HdMonan> HdMonans { get; set; } = new List<HdMonan>();

    public virtual ICollection<HdVe> HdVes { get; set; } = new List<HdVe>();
}

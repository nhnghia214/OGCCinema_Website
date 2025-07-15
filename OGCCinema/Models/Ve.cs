using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Ve
{
    public int Id { get; set; }

    public int IdhoaDonVe { get; set; }

    public int IdlichChieu { get; set; }

    public int Idghe { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaVe { get; set; }

    public DateTime? GioChieu { get; set; }

    public DateTime? NgayMua { get; set; }

    public virtual Ghe IdgheNavigation { get; set; } = null!;

    public virtual HdVe IdhoaDonVeNavigation { get; set; } = null!;

    public virtual Lichchieu IdlichChieuNavigation { get; set; } = null!;
}

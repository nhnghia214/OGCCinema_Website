using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Phongchieu
{
    public int Id { get; set; }

    public string TenPhong { get; set; } = null!;

    public int? TrangThai { get; set; }

    public int MaLoaiPhong { get; set; }

    public string? AnhPhong { get; set; }

    public virtual ICollection<Ghe> Ghes { get; set; } = new List<Ghe>();

    public virtual ICollection<Lichchieu> Lichchieus { get; set; } = new List<Lichchieu>();

    public virtual Loaiphong MaLoaiPhongNavigation { get; set; } = null!;
}

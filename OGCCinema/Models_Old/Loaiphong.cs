using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Loaiphong
{
    public int Id { get; set; }

    public string TenLoaiPhong { get; set; } = null!;

    public int SucChua { get; set; }

    public int SoHang { get; set; }

    public int SoCot { get; set; }

    public virtual ICollection<Phongchieu> Phongchieus { get; set; } = new List<Phongchieu>();
}

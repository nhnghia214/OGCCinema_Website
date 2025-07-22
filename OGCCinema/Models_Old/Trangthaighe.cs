using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Trangthaighe
{
    public int Id { get; set; }

    public int Idghe { get; set; }

    public DateOnly NgayChieu { get; set; }

    public TimeOnly GioChieu { get; set; }

    public int? TrangThai { get; set; }

    public virtual Ghe IdgheNavigation { get; set; } = null!;
}

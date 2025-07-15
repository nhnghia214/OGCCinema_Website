using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Loaimonan
{
    public int Id { get; set; }

    public string TenLoai { get; set; } = null!;

    public virtual ICollection<Monan> Monans { get; set; } = new List<Monan>();
}

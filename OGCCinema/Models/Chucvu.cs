using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Chucvu
{
    public int Id { get; set; }

    public string? TenChucVu { get; set; }

    public virtual ICollection<LogNhanvien> LogNhanviens { get; set; } = new List<LogNhanvien>();

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}

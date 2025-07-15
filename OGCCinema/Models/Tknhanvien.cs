using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Tknhanvien
{
    public string Username { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string? TrangThai { get; set; }

    public virtual ICollection<LogNhanvien> LogNhanviens { get; set; } = new List<LogNhanvien>();

    public virtual Nhanvien? Nhanvien { get; set; }
}

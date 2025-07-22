using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class LogNhanvien
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public int? IdchucVu { get; set; }

    public string? ThaoTac { get; set; }

    public DateTime? ThoiGian { get; set; }

    public string? ChucNang { get; set; }

    public virtual Chucvu? IdchucVuNavigation { get; set; }

    public virtual Tknhanvien? UsernameNavigation { get; set; }
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Lichchieu
{
    public int Id { get; set; }

    public int Idphim { get; set; }

    public string? TenPhim { get; set; }

    public string? TenPhong { get; set; }

    public int Idphong { get; set; }

    public DateTime NgayGio { get; set; }

    public decimal GiaVe { get; set; }

    public string? DiaDiem { get; set; }

    public virtual Phim IdphimNavigation { get; set; } = null!;

    public virtual Phongchieu IdphongNavigation { get; set; } = null!;

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}

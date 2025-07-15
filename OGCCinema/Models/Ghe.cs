using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Ghe
{
    public int Id { get; set; }

    public int Idphong { get; set; }

    public string MaGhe { get; set; } = null!;

    public int? TrangThai { get; set; }

    public virtual Phongchieu IdphongNavigation { get; set; } = null!;

    public virtual ICollection<Trangthaighe> Trangthaighes { get; set; } = new List<Trangthaighe>();

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Dinhdangphim
{
    public int Id { get; set; }

    public string TenDinhDang { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Dotuoi
{
    public string Id { get; set; } = null!;

    public string TenDoTuoi { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}

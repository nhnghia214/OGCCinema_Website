using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Theloaiphim
{
    public int Id { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}

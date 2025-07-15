using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Tkkhachhang
{
    public string Username { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual Khachhang? Khachhang { get; set; }
}

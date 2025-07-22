using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Monan
{
    public int Id { get; set; }

    public string TenMonAn { get; set; } = null!;

    public int IdloaiMonAn { get; set; }

    public decimal? Gia { get; set; }

    public string? MoTa { get; set; }

    public string? Anh { get; set; }

    public virtual ICollection<CthdMonan> CthdMonans { get; set; } = new List<CthdMonan>();

    public virtual Loaimonan IdloaiMonAnNavigation { get; set; } = null!;

    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();
}

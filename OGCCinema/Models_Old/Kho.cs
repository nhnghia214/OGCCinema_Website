using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Kho
{
    public int Id { get; set; }

    public int? IdmonAn { get; set; }

    public int SoLuongTon { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? TenMonAn { get; set; }

    public virtual Monan? IdmonAnNavigation { get; set; }
}

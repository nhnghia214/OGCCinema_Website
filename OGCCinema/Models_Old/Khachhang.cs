using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Khachhang
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? HoTen { get; set; }

    public int? Tuoi { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public int? DiemThuong { get; set; }

    public virtual ICollection<HdMonan> HdMonans { get; set; } = new List<HdMonan>();

    public virtual ICollection<HdVe> HdVes { get; set; } = new List<HdVe>();

    public virtual Tkkhachhang UsernameNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class Nhanvien
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public int? IdchucVu { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public DateOnly? NgayVao { get; set; }

    public string? GioiTinh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? AnhNv { get; set; }

    public virtual ICollection<HdMonan> HdMonans { get; set; } = new List<HdMonan>();

    public virtual ICollection<HdVe> HdVes { get; set; } = new List<HdVe>();

    public virtual Chucvu? IdchucVuNavigation { get; set; }

    public virtual Tknhanvien UsernameNavigation { get; set; } = null!;
}

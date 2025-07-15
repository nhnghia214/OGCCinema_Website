using System;
using System.Collections.Generic;

namespace OGCCinema.Models;

public partial class OtpXacnhan
{
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? MaOtp { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public string? TrangThai { get; set; }
}

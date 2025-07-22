using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models;

public partial class CthdMonan
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string? TenMonAn { get; set; }
    [Column(TypeName = "decimal(8,0)")]
    public decimal? Gia { get; set; }
    [Required]
    public int SoLuong { get; set; }

    public DateTime? NgayMua { get; set; }
    [Required]
    public int IdhoaDon { get; set; }

    public int? IdmonAn { get; set; }
    [StringLength(20)]
    public string? TrangThai { get; set; }

    public virtual HdMonan IdhoaDonNavigation { get; set; } = null!;

    public virtual Monan? IdmonAnNavigation { get; set; }
}


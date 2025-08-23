using OGCCinema.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models_Old
{
    public class Danhgiaphim
    {
        [Key]
        public int Id { get; set; }

        public int? Idphim { get; set; }

        public int? IdkhachHang { get; set; }

        [Required]
        public string NoiDung { get; set; } = null!;

        [Range(1, 10)]
        public int? DiemDanhGia { get; set; }

        public int? ParentId { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime? LanCuoiBinhLuan { get; set; }

        // Navigation properties
        [ForeignKey("Idphim")]
        public virtual Phim? Phim { get; set; }

        [ForeignKey("IdkhachHang")]
        public virtual Khachhang? KhachHang { get; set; }

        [ForeignKey("ParentId")]
        public virtual Danhgiaphim? Parent { get; set; }

        public virtual ICollection<Danhgiaphim> Replies { get; set; } = new List<Danhgiaphim>();
    }
}

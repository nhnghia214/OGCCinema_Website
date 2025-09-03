using OGCCinema.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models_Old
{
    public class DanhGiaNhanVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("KhachHang")]
        public int IdKhachHang { get; set; }

        [Required]
        [ForeignKey("CaLam")]
        public int IdCa { get; set; }

        [Required]
        public DateTime NgayLam { get; set; }

        public string TenNhanVien { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Khachhang KhachHang { get; set; }
        public virtual CaLam CaLam { get; set; }
    }
}

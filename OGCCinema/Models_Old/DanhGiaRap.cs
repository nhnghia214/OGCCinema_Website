using OGCCinema.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models_Old
{
    public class DanhGiaRap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("KhachHang")]
        public int IdKhachHang { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Navigation property
        public virtual Khachhang KhachHang { get; set; }
    }
}

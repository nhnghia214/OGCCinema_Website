using OGCCinema.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OGCCinema.Models_Old
{
    public class LichLam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("NhanVien")]
        public int IdNhanVien { get; set; }

        [Required]
        [ForeignKey("CaLam")]
        public int IdCa { get; set; }

        [Required]
        public DateTime NgayLam { get; set; }

        // Navigation properties
        public virtual Nhanvien NhanVien { get; set; }
        public virtual CaLam CaLam { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace OGCCinema.Models_Old
{
    public class CaLam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TenCa { get; set; }
        [Required]
        public string GioLam { get; set; }
    }
}

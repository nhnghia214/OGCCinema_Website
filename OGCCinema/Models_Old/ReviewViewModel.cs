namespace OGCCinema.Models_Old
{
    public class ReviewViewModel
    {
        public List<CaLam> CaLams { get; set; }
        public int IdKhachHang { get; set; }
        public int IdCa { get; set; }
        public DateTime NgayLam { get; set; }
        public string TenNhanVien { get; set; }
        public string NoiDungRap { get; set; }
        public string NoiDungNhanVien { get; set; }
    }
}

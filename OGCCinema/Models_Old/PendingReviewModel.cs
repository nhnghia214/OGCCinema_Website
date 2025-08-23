namespace OGCCinema.Models_Old
{
    public class PendingReviewModel
    {
        public int PhimID { get; set; }
        public int Rating { get; set; }
        public string? NoiDung { get; set; }
        public int? ParentID { get; set; } // Thêm để hỗ trợ trả lời
    }
}

namespace Fantasy_Encylopedia_App.Models
{
    public class SearchLog
    {
        public int Id { get; set; }
        public string QueryText { get; set; } = "";
        public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
    }
}

namespace Fantasy_Encylopedia_App.Models
{
    public class SearchLogResult
    {
        public int Id { get; set; }
        public string ComicTitle { get; set; } = "";
        public DateTime ReturnedAt { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
    }
}

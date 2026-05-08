using Microsoft.AspNetCore.Identity;

namespace Fantasy_Encylopedia_App.Models
{
    public class FlaggedComic
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Genre { get; set; } = "";
        public string ISBN { get; set; } = "";
        public string Publisher { get; set; } = "";
        public string DateOfPublication { get; set; } = "";
        public string Language { get; set; } = "";
        public string NameType { get; set; } = "";

        public string FlagReason { get; set; } = "Marked for review";
        public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

        public string StaffUserId { get; set; } = "";
        public IdentityUser? StaffUser { get; set; }
    }
}

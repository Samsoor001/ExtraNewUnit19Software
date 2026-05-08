using Microsoft.AspNetCore.Identity;

namespace Fantasy_Encylopedia_App.Models
{
    public class SaveComic
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string DateOfPublication { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string NameType { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }
    }
}

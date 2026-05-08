using System.Collections.Generic;
using Fantasy_Encyclopedia.Core.Models;

namespace Fantasy_Encylopedia_App.Models
{
    public class ComicSearchViewModel
    {
        public string? SelectedGenre { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedLanguage { get; set; }
        public string? SelectedNameType { get; set; }

        public string? AuthorQuery { get; set; }
      
        public string? TitleQuery { get; set; }

        public string SortTitle { get; set; } = "az"; 
 
        public string GroupBy { get; set; } = "none"; 
    
        public List<string> Genres { get; set; } = new();
        public List<int> Years { get; set; } = new();
        public List<string> Languages { get; set; } = new();
        public List<string> NameTypes { get; set; } = new();

        public List<Books> Results { get; set; } = new();

        public Dictionary<string, List<Books>> Grouped { get; set; } = new();
    }
}
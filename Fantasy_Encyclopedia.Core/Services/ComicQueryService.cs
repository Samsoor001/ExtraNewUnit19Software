using Fantasy_Encyclopedia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_Encyclopedia.Core.Services
{
    public sealed class ComicQueryService
    {
        //dropdown lists (Genres/Years/Languages/NameTypes) from ALL cached records
        public ComicSearchOptions BuildOptions(List<Books> all)
        {
            return new ComicSearchOptions
            {
                Genres = all.Select(x => x.Genre).Where(NotEmpty).Select(Trim)
                    .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList(),

                Years = all.Select(x => GetYear(x.DateOfPublication)).Where(y => y.HasValue).Select(y => y!.Value)
                    .Distinct().OrderBy(y => y).ToList(),

                Languages = all.Select(x => x.Languages).Where(NotEmpty).Select(Trim)
                    .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList(),

                NameTypes = all.Select(x => x.TypeOfName).Where(NotEmpty).Select(Trim)
                    .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList(),
            };
        }

        
        public ComicQueryResult Query(
            IEnumerable<Books> all,
            ComicQueryInput input,
            int skip,
            int take)
        {
            if (all == null) throw new ArgumentNullException(nameof(all));
            input ??= new ComicQueryInput();

            IEnumerable<Books> q = all;

            
            if (!string.IsNullOrWhiteSpace(input.TitleQuery))
            {
                var t = input.TitleQuery.Trim();
                q = q.Where(b => (b.Title ?? "").Contains(t, StringComparison.OrdinalIgnoreCase));
            }

            
            if (!string.IsNullOrWhiteSpace(input.AuthorQuery))
            {
                var a = input.AuthorQuery.Trim();
                q = q.Where(b =>
                    (b.Name ?? "").Contains(a, StringComparison.OrdinalIgnoreCase) ||
                    (b.OtherNames ?? "").Contains(a, StringComparison.OrdinalIgnoreCase));
            }

            
            if (!string.IsNullOrWhiteSpace(input.SelectedGenre))
            {
                var g = input.SelectedGenre.Trim();
                q = q.Where(b => string.Equals((b.Genre ?? "").Trim(), g, StringComparison.OrdinalIgnoreCase));
            }

            
            if (input.SelectedYear.HasValue)
            {
                var year = input.SelectedYear.Value;
                q = q.Where(b => GetYear(b.DateOfPublication) == year);
            }

            
            if (!string.IsNullOrWhiteSpace(input.SelectedLanguage))
            {
                var l = input.SelectedLanguage.Trim();
                q = q.Where(b => (b.Languages ?? "").Contains(l, StringComparison.OrdinalIgnoreCase));
            }

            
            if (!string.IsNullOrWhiteSpace(input.SelectedNameType))
            {
                var nt = input.SelectedNameType.Trim();
                q = q.Where(b => string.Equals((b.TypeOfName ?? "").Trim(), nt, StringComparison.OrdinalIgnoreCase));
            }

            
            q = input.SortTitle == "za"
                ? q.OrderByDescending(b => b.Title)
                : q.OrderBy(b => b.Title);

            
            var filtered = q.ToList();

            
            if (string.Equals(input.GroupBy, "year", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = filtered
                    .GroupBy(b => (GetYear(b.DateOfPublication)?.ToString() ?? "Unknown Year"))
                    .OrderBy(gp => gp.Key)
                    .ToDictionary(gp => gp.Key, gp => gp.ToList());

                return new ComicQueryResult
                {
                    TotalMatches = filtered.Count,
                    GroupedByKey = grouped,
                    Page = new List<Books>()
                };
            }

            
            return new ComicQueryResult
            {
                TotalMatches = filtered.Count,
                Page = filtered.Skip(skip).Take(take).ToList(),
                GroupedByKey = new Dictionary<string, List<Books>>()
            };
        }

        

        public static int? GetYear(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();

            
            for (int i = 0; i <= s.Length - 4; i++)
            {
                if (char.IsDigit(s[i]) && char.IsDigit(s[i + 1]) && char.IsDigit(s[i + 2]) && char.IsDigit(s[i + 3]))
                {
                    if (int.TryParse(s.Substring(i, 4), out var y) && y >= 1000 && y <= 2100)
                        return y;
                }
            }
            return null;
        }

        private static bool NotEmpty(string? s) => !string.IsNullOrWhiteSpace(s);
        private static string Trim(string s) => s.Trim();
    }

    
    public class ComicQueryInput
    {
        public string? TitleQuery { get; set; }
        public string? AuthorQuery { get; set; }
        public string? SelectedGenre { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedLanguage { get; set; }
        public string? SelectedNameType { get; set; }
        public string SortTitle { get; set; } = "az";   
        public string GroupBy { get; set; } = "none";   
    }

    // Dropdown options
    public class ComicSearchOptions
    {
        public List<string> Genres { get; set; } = new();
        public List<int> Years { get; set; } = new();
        public List<string> Languages { get; set; } = new();
        public List<string> NameTypes { get; set; } = new();
    }

    // Output of a query
    public class ComicQueryResult
    {
        public int TotalMatches { get; set; }
        public List<Books> Page { get; set; } = new();
        public Dictionary<string, List<Books>> GroupedByKey { get; set; } = new();
    }
}

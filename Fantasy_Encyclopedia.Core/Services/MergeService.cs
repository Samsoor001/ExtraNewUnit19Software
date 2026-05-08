using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Fantasy_Encyclopedia.Core.Models;

namespace Fantasy_Encyclopedia.Core.Services
{
    public static class MergeService
    {
        public static List<Books> MergeDuplicateTitles(IEnumerable<Books> records) 
        {
            var dict = new Dictionary<string, Books>(StringComparer.OrdinalIgnoreCase);
            var dupCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var r in records)
            {
                var key = NormalizeTitle(r?.Title);

                
                if (string.IsNullOrWhiteSpace(key))
                    key = $"__EMPTY_TITLE__:{Guid.NewGuid():N}";

                if (!dict.TryGetValue(key, out var existing))
                {
                    dict[key] = r!;
                    dupCount[key] = 1;
                }
                else
                {
                    dict[key] = Merge(existing, r!);
                    dupCount[key] = dupCount[key] + 1;
                }
            }

            
            foreach (var kv in dupCount)
            {
                if (kv.Value > 1 && dict.TryGetValue(kv.Key, out var b))
                {
                    b.Notes = CombineDistinct(b.Notes, $"Merged {kv.Value} records", separator: " | ");
                }
            }

            return dict.Values.ToList();
        }

        private static Books Merge(Books a, Books b)
        { 
            
            a.Name = FirstNonEmpty(a.Name, b.Name);
            a.DatesAssociatedWithName = FirstNonEmpty(a.DatesAssociatedWithName, b.DatesAssociatedWithName);
            a.TypeOfName = FirstNonEmpty(a.TypeOfName, b.TypeOfName);
            a.Role = FirstNonEmpty(a.Role, b.Role);

            a.OtherNames = CombineDistinct(a.OtherNames, b.OtherNames);
            a.BLRecordID = CombineDistinct(a.BLRecordID, b.BLRecordID);
            a.TypeOfResource = FirstNonEmpty(a.TypeOfResource, b.TypeOfResource);
            a.ContentType = FirstNonEmpty(a.ContentType, b.ContentType);
            a.MaterialType = FirstNonEmpty(a.MaterialType, b.MaterialType);

            a.BNBNumber = CombineDistinct(a.BNBNumber, b.BNBNumber);
            a.ISBN = CombineDistinct(a.ISBN, b.ISBN);

            a.Title = FirstNonEmpty(a.Title, b.Title);
            a.VariantTitles = CombineDistinct(a.VariantTitles, b.VariantTitles);
            a.SeriesTitle = FirstNonEmpty(a.SeriesTitle, b.SeriesTitle);
            a.NumberWithinSeries = FirstNonEmpty(a.NumberWithinSeries, b.NumberWithinSeries);

            a.CountryOfPublication = FirstNonEmpty(a.CountryOfPublication, b.CountryOfPublication);
            a.PlaceOfPublication = FirstNonEmpty(a.PlaceOfPublication, b.PlaceOfPublication);
            a.Publisher = FirstNonEmpty(a.Publisher, b.Publisher);

            
            a.DateOfPublication = CombineDistinct(a.DateOfPublication, b.DateOfPublication);

            a.Edition = CombineDistinct(a.Edition, b.Edition);
            a.Description = FirstNonEmpty(a.Description, b.Description);

            a.DeweyClassification = CombineDistinct(a.DeweyClassification, b.DeweyClassification);
            a.BLShelfMark = CombineDistinct(a.BLShelfMark, b.BLShelfMark);

            a.Topics = CombineDistinct(a.Topics, b.Topics);
            a.Genre = CombineDistinct(a.Genre, b.Genre);
            a.Languages = CombineDistinct(a.Languages, b.Languages);

            a.Notes = CombineDistinct(a.Notes, b.Notes);

            return a;
        }

        private static string NormalizeTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "";
            var t = title.Trim();
            // collapse multiple spaces
            t = Regex.Replace(t, @"\s+", " ");
            return t.ToLowerInvariant();
        }

        private static string FirstNonEmpty(string? a, string? b)
            => !string.IsNullOrWhiteSpace(a) ? a! : (!string.IsNullOrWhiteSpace(b) ? b! : "");

        private static string CombineDistinct(string? a, string? b, string separator = "; ")
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void add(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return;

                // Split common multi-value formats
                foreach (var part in s.Split(new[] { ';', '|', ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var p = part.Trim();
                    if (!string.IsNullOrWhiteSpace(p))
                        set.Add(p);
                }
            }

            add(a);
            add(b);

            return string.Join(separator, set);

        }
    }
}

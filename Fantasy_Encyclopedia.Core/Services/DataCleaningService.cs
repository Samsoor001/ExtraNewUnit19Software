using Fantasy_Encyclopedia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_Encyclopedia.Core.Services
{
    public static class DataCleaningService
    {
        public static List<Books> CleanRecords(List<Books> records)
        {
            foreach (var r in records)
            {
                r.Title = FillIfMissing(r.Title, "missing title");
                r.Name = FillIfMissing(r.Name, "missing author");
                r.ISBN = FillIfMissing(r.ISBN, "missing ISBN");
                r.Publisher = FillIfMissing(r.Publisher, "missing publisher");
                r.Genre = FillIfMissing(r.Genre, "missing genre");
                r.Languages = FillIfMissing(r.Languages, "missing language");
    }

            return records;
        }

private static string FillIfMissing(string? value, string replacement)
{
    return string.IsNullOrWhiteSpace(value) ? replacement : value;
}
    }
}

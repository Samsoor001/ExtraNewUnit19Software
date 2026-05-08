using Fantasy_Encyclopedia.Core.Models;
using Fantasy_Encyclopedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTesting
{
    public class DataCleaningServiceTests
    {
        [Fact]
        public void CleanRecords_BlankIsbn_ReplacesWithMissingIsbn()
        {
            var records = new List<Books>
            {
                new Books
                {
                    Title = "Batman",
                    Name = "Bob Smith",
                    ISBN = "",
                    Publisher = "DC",
                    Genre = "Fantasy",
                    Languages = "English"
                }
            };

            var result = DataCleaningService.CleanRecords(records);

            Assert.Single(result);
            Assert.Equal("missing ISBN", result[0].ISBN);
        }

        [Fact]
        public void CleanRecords_BlankTitle_ReplacesWithMissingTitle()
        {
            var records = new List<Books>
            {
                new Books
                {
                    Title = "",
                    Name = "Bob Smith",
                    ISBN = "12345",
                    Publisher = "DC",
                    Genre = "Fantasy",
                    Languages = "English"
                }
            };

            var result = DataCleaningService.CleanRecords(records);

            Assert.Equal("missing title", result[0].Title);
        }

        [Fact]
        public void CleanRecords_BlankGenre_ReplacesWithMissingGenre()
        {
            var records = new List<Books>
            {
                new Books
                {
                    Title = "Batman",
                    Name = "Bob Smith",
                    ISBN = "12345",
                    Publisher = "DC",
                    Genre = "",
                    Languages = "English"
                }
            };

            var result = DataCleaningService.CleanRecords(records);

            Assert.Equal("missing genre", result[0].Genre);
        }
    }
}

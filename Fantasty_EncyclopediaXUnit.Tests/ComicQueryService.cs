using Fantasy_Encyclopedia.Core.Models;
using Fantasy_Encyclopedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTesting
{
    public class ComicQueryServiceTests
    {
        private readonly ComicQueryService _service = new ComicQueryService();

        private readonly List<Books> _books = new List<Books>
        {
            new Books
            {
                Title = "Batman",
                Name = "Bob Kane",
                Genre = "Fantasy",
                Languages = "English",
                TypeOfName = "Personal",
                DateOfPublication = "2020"
            },
            new Books
            {
                Title = "Batman Beyond",
                Name = "Dan Jurgens",
                Genre = "Fantasy",
                Languages = "English",
                TypeOfName = "Personal",
                DateOfPublication = "2021"
            },
            new Books
            {
                Title = "Superman",
                Name = "Jerry Siegel",
                Genre = "Action",
                Languages = "French",
                TypeOfName = "Corporate",
                DateOfPublication = "2019"
            }
        };

        [Fact]
        public void Query_TitleSearch_ReturnsOnlyMatchingRecords()
        {
            var input = new ComicQueryInput
            {
                TitleQuery = "Batman"
            };

            var result = _service.Query(_books, input, 0, 10);

            Assert.Equal(2, result.TotalMatches);
            Assert.All(result.Page, x => Assert.Contains("Batman", x.Title));
        }

        [Fact]
        public void Query_GenreFilter_ReturnsOnlyMatchingGenreResults()
        {
            var input = new ComicQueryInput
            {
                SelectedGenre = "Fantasy"
            };

            var result = _service.Query(_books, input, 0, 10);

            Assert.Equal(2, result.TotalMatches);
            Assert.All(result.Page, x => Assert.Equal("Fantasy", x.Genre));
        }

        [Fact]
        public void Query_SortTitleZa_ReturnsReverseAlphabeticalOrder()
        {
            var input = new ComicQueryInput
            {
                SortTitle = "za"
            };

            var result = _service.Query(_books, input, 0, 10);

            Assert.Equal("Superman", result.Page[0].Title);
            Assert.Equal("Batman Beyond", result.Page[1].Title);
            Assert.Equal("Batman", result.Page[2].Title);
        }

        [Fact]
        public void Query_BlankInput_ReturnsAllRecords()
        {
            var input = new ComicQueryInput();

            var result = _service.Query(_books, input, 0, 10);

            Assert.Equal(3, result.TotalMatches);
            Assert.Equal(3, result.Page.Count);
        }
    }
}

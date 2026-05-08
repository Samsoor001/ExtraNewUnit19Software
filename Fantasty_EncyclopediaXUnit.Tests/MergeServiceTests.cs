using Fantasy_Encyclopedia.Core.Models;
using Fantasy_Encyclopedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTesting
{
    public class MergeServiceTests
    {
        [Fact]
        public void MergeDuplicateTitles_DuplicateTitles_ReturnsSingleMergedRecord()
        {
            var records = new List<Books>
            {
                new Books { Title = "Batman", ISBN = "111", Genre = "Fantasy" },
                new Books { Title = "Batman", ISBN = "222", Genre = "Adventure" }
            };

            var result = MergeService.MergeDuplicateTitles(records);

            Assert.Single(result);
            Assert.Equal("Batman", result[0].Title);
        }

        [Fact]
        public void MergeDuplicateTitles_UniqueTitles_RemainSeparate()
        {
            var records = new List<Books>
            {
                new Books { Title = "Batman", ISBN = "111" },
                new Books { Title = "Superman", ISBN = "222" }
            };

            var result = MergeService.MergeDuplicateTitles(records);

            Assert.Equal(2, result.Count);
        }
    }
}

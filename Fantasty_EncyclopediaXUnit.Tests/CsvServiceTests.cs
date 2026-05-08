using Fantasy_Encyclopedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTesting
{
    public class CsvServiceTests
    {
        [Fact]
        public void ReadCsvFile_ValidCsv_ReturnsBooksList()
        {
            var csvContent =
                "Name,Dates associated with name,Type of name,Role,Other names,BL record ID,Type of resource,Content type,Material type," +
                "BNB number,ISBN,Title,Variant titles,Series title,Number within series,Country of publication,Place of publication,Publisher," +
                "Date of publication,Edition,Physical description,Dewey classification,BL shelfmark,Topics,Genre,Languages,Notes\n" +
                "Bob Smith,,Personal,Author,,1,Book,Text,Print,BNB1,12345,Batman,,,,UK,London,DC,2020,,,741.5,,Heroes,Fantasy,English,\n";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvService();

            var result = service.ReadCsvFile(stream).ToList();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Batman", result[0].Title);
        }

        [Fact]
        public void ReadCsvFile_EmptyCsvWithHeaders_ReturnsEmptyList()
        {
            var csvContent =
                "Name,Dates associated with name,Type of name,Role,Other names,BL record ID,Type of resource,Content type,Material type,BNB number,ISBN" +
                ",Title,Variant titles,Series title,Number within series,Country of publication,Place of publication,Publisher,Date of publication,Edition" +
                ",Physical description,Dewey classification,BL shelfmark,Topics,Genre,Languages,Notes\n";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvService();

            var result = service.ReadCsvFile(stream).ToList();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ReadCsvFile_BlankIsbn_IsCleanedToMissingIsbn()
        {
            var csvContent =
                "Name,Dates associated with name,Type of name,Role,Other names,BL record ID,Type of resource,Content type,Material type" +
                ",BNB number,ISBN,Title,Variant titles,Series title,Number within series,Country of publication,Place of publication,Publisher" +
                ",Date of publication,Edition,Physical description,Dewey classification,BL shelfmark,Topics,Genre,Languages,Notes\n" +
                "Bob Smith,,Personal,Author,,1,Book,Text,Print,BNB1,,Batman,,,,UK,London,DC,2020,,,741.5,,Heroes,Fantasy,English,\n";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvService();

            var result = service.ReadCsvFile(stream).ToList();

            Assert.Single(result);
            Assert.Equal("missing ISBN", result[0].ISBN);
        }
    }
}

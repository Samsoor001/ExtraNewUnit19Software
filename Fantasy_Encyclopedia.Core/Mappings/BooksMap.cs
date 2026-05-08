using CsvHelper.Configuration;
using Fantasy_Encyclopedia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_Encyclopedia.Core.Mappings
{
    internal class BooksMap : ClassMap<Books>
    {
        public BooksMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.DatesAssociatedWithName).Name("Dates associated with name");
            Map(m => m.TypeOfName).Name("Type of name");
            Map(m => m.Role).Name("Role");
            Map(m => m.OtherNames).Name("Other names");
            Map(m => m.BLRecordID).Name("BL record ID");
            Map(m => m.TypeOfResource).Name("Type of resource");
            Map(m => m.ContentType).Name("Content type");
            Map(m => m.MaterialType).Name("Material type");
            Map(m => m.BNBNumber).Name("BNB number");
            Map(m => m.ISBN).Name("ISBN");
            Map(m => m.Title).Name("Title");
            Map(m => m.VariantTitles).Name("Variant titles");
            Map(m => m.SeriesTitle).Name("Series title");
            Map(m => m.NumberWithinSeries).Name("Number within series");
            Map(m => m.CountryOfPublication).Name("Country of publication");
            Map(m => m.PlaceOfPublication).Name("Place of publication");
            Map(m => m.Publisher).Name("Publisher");
            Map(m => m.DateOfPublication).Name("Date of publication");
            Map(m => m.Edition).Name("Edition");
            Map(m => m.Description).Name("Physical description");
            Map(m => m.DeweyClassification).Name("Dewey classification");
            Map(m => m.BLShelfMark).Name("BL shelfmark");
            Map(m => m.Topics).Name("Topics");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.Languages).Name("Languages");
            Map(m => m.Notes).Name("Notes");
        }
    }
}

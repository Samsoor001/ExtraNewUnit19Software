using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy_Encyclopedia.Core.Models
{
    public class Books
    {
        public string Name { get; set; }
        public string DatesAssociatedWithName { get; set; }
        public string TypeOfName { get; set; }
        public string Role { get; set; }
        public string OtherNames { get; set; }
        public string BLRecordID { get; set; }
        public string TypeOfResource { get; set; }
        public string ContentType { get; set; }
        public string MaterialType { get; set; }
        public string BNBNumber { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string VariantTitles { get; set; }
        public string SeriesTitle { get; set; }
        public string NumberWithinSeries { get; set; }
        public string CountryOfPublication { get; set; }
        public string PlaceOfPublication { get; set; }
        public string Publisher { get; set; }
        public string DateOfPublication { get; set; }
        public string Edition { get; set; }
        public string Description { get; set; }
        public string DeweyClassification { get; set; }
        public string BLShelfMark { get; set; }
        public string Topics { get; set; }
        public string Genre { get; set; }
        public string Languages { get; set; }
        public string Notes { get; set; }
    }
}

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Fantasy_Encyclopedia.Core.Models;
using System.Globalization;
using Fantasy_Encyclopedia.Core.Mappings;

namespace Fantasy_Encyclopedia.Core.Services
{
    public class CsvService
    {
        public IEnumerable<Books> ReadCsvFile(Stream fileStream)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header?.Trim(),
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using var reader = new StreamReader(fileStream);
                using var csv = new CsvReader(reader, config);

                csv.Context.RegisterClassMap<BooksMap>();

                var records = csv.GetRecords<Books>().ToList();
                records = DataCleaningService.CleanRecords(records);
                return MergeService.MergeDuplicateTitles(records);
            }
            catch (CsvHelper.HeaderValidationException ex)
            {
                throw new ApplicationException("CSV file header is invalid (columns don't match your model).", ex);
            }
            catch (TypeConverterException ex)
            {
                throw new ApplicationException("CSV file contains an invalid data format.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading CSV file.", ex);
            }
        }
    }
}
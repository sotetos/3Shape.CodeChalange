using Models;
using Models.Text;
using Services.Internals.ExtensionMethods;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Internals
{
    public class DataImporter : IDataImporter
    {
        private readonly IImportDataParser _importDataParser;
        private readonly ISearchStringParser _searchStringParser;
        private readonly PretendBookDataSource _pretendBookDataSource;

        public DataImporter(IImportDataParser importDataParser, ISearchStringParser searchStringParser, PretendBookDataSource pretendBookDataSource)
        {
            ArgumentNullException.ThrowIfNull(importDataParser);
            ArgumentNullException.ThrowIfNull(searchStringParser);
            ArgumentNullException.ThrowIfNull(pretendBookDataSource);

            _importDataParser = importDataParser;
            _searchStringParser = searchStringParser;
            _pretendBookDataSource = pretendBookDataSource;
        }

        public List<Book> ReadBooks(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var data = _importDataParser.ParseInput(input);

            var results = data
                .Where(d => !d.HasErrors)
                .Select(BuildLibraryItem);

            //For this example, I will use only non-null values due to only implementing books
            var books = results.Where(r => r != null);

            return books
                .Where(r => r.LibraryItemType == LibraryItemType.Book)
                .Select(r => (Book)r)
                .ToList();
        }

        public List<Book> FindBooks(string searchString)
        {
            return new List<Book>(1);
        }

        private static LibraryItemBase? BuildLibraryItem(ParsedInputData data) => data.InputType switch
        {
            LibraryItemType.Book => BuildBook(data),
            LibraryItemType.CD => null,
            LibraryItemType.DVD => null,
            LibraryItemType.BlueRay => null,
            LibraryItemType.EBook => null,
            LibraryItemType.AudioFile => null,
            LibraryItemType.VideoFile => null,
            LibraryItemType.Other => null,
            _ => throw new ArgumentOutOfRangeException()
        };

        private static Book BuildBook(ParsedInputData data)
        {
            var result = new Book()
            {
                Id = Guid.NewGuid()
            };
            foreach (var propertyValuePair in data.PropertyValueData)
            {
                result.ParsePropertyValuePair(propertyValuePair);
            }
            return result;
        }
    }
}

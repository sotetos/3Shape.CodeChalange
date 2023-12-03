using Models;
using Models.Text;
using Services.DTOs;
using Services.Internals.ExtensionMethods;
using Services.Ports;

namespace Services.Internals
{
    internal class DataService : IDataImporter, IdataAccessor
    {
        private readonly IImportDataParser _importDataParser;
        private readonly ISearchStringParser _searchStringParser;
        private readonly PretendBookDataSource _pretendBookDataSource;

        public DataService(IImportDataParser importDataParser, ISearchStringParser searchStringParser, PretendBookDataSource pretendBookDataSource)
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

            _pretendBookDataSource.Books.AddRange(books
                .Where(r => r.LibraryItemType == LibraryItemType.Book)
                .Select(r => (Book)r)
            );

            return books
                .Where(r => r.LibraryItemType == LibraryItemType.Book)
                .Select(r => (Book)r)
                .ToList();
        }

        public List<Book> FindBooks(string search)
        {
            ArgumentNullException.ThrowIfNull(search);

            var conditions = _searchStringParser.ParseSearchString(search);

            if (!conditions.Any())
            {
                return _pretendBookDataSource.Books;
            }

            var sourceBooks = _pretendBookDataSource.Books;
            IEnumerable<Book> resultBooks = new List<Book>();

            resultBooks = conditions.Aggregate(resultBooks,
                (current, condition) => 
                    current.Union(sourceBooks.Where(s => s.Search(condition)))
                );

            return resultBooks.ToList();
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

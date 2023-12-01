using Models.Text;
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
            _importDataParser = importDataParser;
            _searchStringParser = searchStringParser;
            _pretendBookDataSource = pretendBookDataSource;
        }

        public List<Book> ReadBooks(string input)
        {
            return new List<Book>(1);
        }

        public List<Book> FindBooks(string searchString)
        {
            return new List<Book>(1);
        }
    }
}

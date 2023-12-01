using Models.Text;

namespace Services.Ports;

public interface IDataImporter
{
    List<Book> ReadBooks(string input);
}
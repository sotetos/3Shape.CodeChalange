using Models.Text;

namespace Services.Ports;

public interface IdataAccessor
{
    List<Book> FindBooks(string search);
}
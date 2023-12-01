using Models.Text;

namespace Services
{
    public class DataImporter
    {
        private List<Book> Books { get; set; } = new List<Book>();

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

using Models.Interfaces;

namespace Models.Text
{
    public class EBook: TextItemBase, IDigitalWork
    {
        public string FileFormat { get; set; } = string.Empty;

        public EBook()
        {
            LibraryItemType = LibraryItemType.EBook;
        }

        public string GetFileFormat() => FileFormat;
    }
}

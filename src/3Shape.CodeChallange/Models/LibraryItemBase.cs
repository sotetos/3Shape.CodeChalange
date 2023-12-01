using Models.Interfaces;

namespace Models
{
    public abstract class LibraryItemBase: IWorkWithTitle
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public LibraryItemType LibraryItemType { get; set; }
    }
}

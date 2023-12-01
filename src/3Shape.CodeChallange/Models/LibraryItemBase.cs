namespace Models
{
    public abstract class LibraryItemBase
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public LibraryItemType LibraryItemType { get; set; }
    }
}

using Models.Interfaces;

namespace Models.Text
{
    public abstract class TextItemBase: LibraryItemBase, IWorkWithCreators
    {
        public List<string> Authors { get; set; } = new List<string>();
        public int PageCount { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public int YearPublished { get; set; }

        public virtual IEnumerable<string> GetCreators() => Authors;
    }
}

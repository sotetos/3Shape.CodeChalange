using Models.Interfaces;

namespace Models
{
    public abstract class TrackBase: IWorkWithCreators, IWorkWithTitle
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int DurationInSeconds { get; set; }
        public virtual IEnumerable<string> GetCreators()
        {
            return new[] { Artist };
        }
    }
}

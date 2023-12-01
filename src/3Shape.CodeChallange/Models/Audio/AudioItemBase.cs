using Models.Audio;
using Models.Interfaces;

namespace Models.Text
{
    public abstract class AudioItemBase: LibraryItemBase, IWorkWithCreators
    {
        public List<AudioTrack> Tracks { get; set; } = new List<AudioTrack>();
        public virtual IEnumerable<string> GetCreators()
        {
            return Tracks.Select(t => t.Artist).Distinct();
        }
    }
}

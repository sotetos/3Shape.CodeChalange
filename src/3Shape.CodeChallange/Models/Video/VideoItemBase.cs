namespace Models.Video
{
    public abstract class VideoItemBase: LibraryItemBase
    {
        public List<VideoTrack> Tracks { get; set; } = new List<VideoTrack>();
        public virtual IEnumerable<string> GetCreators()
        {
            return Tracks.Select(t => t.Artist).Distinct();
        }

    }
}

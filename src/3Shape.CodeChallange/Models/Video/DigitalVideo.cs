using Models.Interfaces;

namespace Models.Video
{
    public class DigitalVideo: VideoItemBase, IDigitalWork
    {
        public string FileFormat { get; set; } = string.Empty;

        public DigitalVideo()
        {
            LibraryItemType = LibraryItemType.VideoFile;
        }
    }
}

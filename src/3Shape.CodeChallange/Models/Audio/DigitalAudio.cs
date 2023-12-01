using Models.Interfaces;
using Models.Text;

namespace Models.Audio
{
    public class DigitalAudio: AudioItemBase, IDigitalWork
    {
        public string FileFormat { get; set; }

        public DigitalAudio()
        {
            LibraryItemType = LibraryItemType.AudioFile;
        }

        public string GetFileFormat() => FileFormat;
    }
}

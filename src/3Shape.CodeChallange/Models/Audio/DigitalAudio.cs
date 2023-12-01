using Models.Interfaces;
using Models.Text;

namespace Models.Audio
{
    public class DigitalAudio: AudioItemBase, IDigitalWork
    {
        public string FileFormat { get; set; } = string.Empty;

        public DigitalAudio()
        {
            LibraryItemType = LibraryItemType.AudioFile;
        }
    }
}

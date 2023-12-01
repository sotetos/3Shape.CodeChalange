using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

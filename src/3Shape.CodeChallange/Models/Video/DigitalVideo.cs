using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Text;

namespace Models.Audio
{
    public class CD: AudioItemBase
    {
        public CD()
        {
            LibraryItemType = LibraryItemType.CD;
        }
    }
}

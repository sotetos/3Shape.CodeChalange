using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Player.Components
{
    struct TrackItem
    {
        public string name { get; set; }
        public string path { get; set; }

        public TrackItem(string _name, string _path)
        {
            this.name = _name;
            this.path = _path;
        }
    }
}

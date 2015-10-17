using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Venue {
        public string Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string Shortcut {
            get; set;
        }

        public uint DistrictId {
            get; set;
        }

        public double Latitude {
            get; set;
        }

        public double Longitude {
            get; set;
        }
    }
}

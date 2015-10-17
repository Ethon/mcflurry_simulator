using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Performance {
        public DateTime Date {
            get; set;
        }

        public uint ArtistId {
            get; set;
        }

        public string VenueId {
            get; set;
        }
    }
}

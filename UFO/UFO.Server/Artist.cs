using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Artist {
        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string EmailAddress {
            get; set;
        }

        public uint CategoryId {
            get; set;
        }

        public uint CountryId {
            get; set;
        }

        public string PicturePath {
            get; set;
        }

        public string VideoPath {
            get; set;
        }
    }
}

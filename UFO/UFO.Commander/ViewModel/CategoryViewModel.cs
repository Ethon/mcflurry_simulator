using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class CategoryViewModel {
        private Category category;

        public CategoryViewModel(Category category) {
            this.category = category;
        }

        public uint Id {
            get;
        }

        public string Name {
            get {
                return category.Name;
            }
        }

        public string Shortcut {
            get {
                return category.Shortcut;
            }
        }

        public override string ToString() {
            return Name;
        }

        public override bool Equals(object obj) {
            if(!(obj is CategoryViewModel)) {
                return false;
            }
            return Name.Equals(())
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class CategoryListViewModel {
        private ICategoryService categoryService;

        public CategoryListViewModel(ICategoryService categoryService) {
            this.categoryService = categoryService;
            Categories = new ObservableCollection<CategoryViewModel>();
            UpdateCategories();
        }

        public ObservableCollection<CategoryViewModel> Categories {
            get; set;
        }

        public void UpdateCategories() {
            Categories.Clear();
            foreach(var cat in categoryService.GetAllCategories()) {
                Categories.Add(new CategoryViewModel(cat));
            }
        }
    }
}

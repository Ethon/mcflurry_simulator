using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander {
    class IdToCategoryConverter : IValueConverter {
        private ICategoryService categoryService = SharedServices.Instance.CategoryService;
        private Dictionary<uint, Category> idCategoryCache = new Dictionary<uint, Category>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Category cat;
            if(!idCategoryCache.TryGetValue((uint)value, out cat)) {
                cat = categoryService.GetCategoryById((uint)value);
                idCategoryCache.Add(cat.Id, cat);
            }
            return cat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((Category)value).Id;
        }
    }
}

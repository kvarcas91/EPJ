using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class BoolToExpandableIconConverter : BaseValueConverter<BoolToExpandableIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isExpanded = (bool)value;
            return isExpanded ? PackIconKind.ExpandLess : PackIconKind.ExpandMore;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

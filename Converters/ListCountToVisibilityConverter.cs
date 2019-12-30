using EPJ.Models.Interfaces;
using EPJ.Models.Project;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EPJ
{
    public class ListCountToVisibilityConverter : BaseValueConverter<ListCountToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mlist = (IList<IProject>)value;
            return mlist.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

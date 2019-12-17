using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EPJ
{
    public class BoolToTaskBackgroundConverter : BaseValueConverter<BoolToTaskBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var color = "white";
            if ((bool)value)
            {
                color = Application.Current.Resources["BackgroundLightBrush"].ToString();
            }
            return color;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

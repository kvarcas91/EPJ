using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    /// <summary>
    /// Converts the <see cref="ApplicationPageConverter"/> to an actual view/page
    /// </summary>
    public class ApplicationPageConverter : BaseValueConverter<ApplicationPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.ProjectList:
                    return new ProjectListPage();
                case ApplicationPage.AddProject:
                    return new AddProjectPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

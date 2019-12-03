using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class EnumHelper
    {
       
        public static List<string> SortByEnumToString
        {
            get
            {
                return GetDescriptions(typeof(SortBy));
                //return Enum.GetNames(typeof(SortBy)).ToList();
            }
        }

        private static List<string> GetDescriptions(Type enumType)
        {

            var output = new List<string>();
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                var field = enumType.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    output.Add(fd.Description);
                }
            }
            return output;
        }

       

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class ValidateUserInput
    {

        public static bool IsNullOrWhiteSpace(params string[] input)
        {
            
            foreach (var item in input)
            {
                if (String.IsNullOrWhiteSpace(item))
                {
                    return false;
                }
            }
            return true;
        }

    }
}

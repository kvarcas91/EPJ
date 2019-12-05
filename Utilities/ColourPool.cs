using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    class ColourPool
    {

        private static readonly string[] Colours = {
            "#B043EF",  // purple
            "#5757ED",  // dark blue
            "#549DEA",  // light blue 
        };

        private static  Stack<string> mColours = new Stack<string>(Colours);

        public static string GetColour ()
        {
            if (mColours.Count() == 0) mColours = new Stack<string>(Colours);
            return mColours.Pop();
        }

    }
}

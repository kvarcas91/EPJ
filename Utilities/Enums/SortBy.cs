using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public enum SortBy
    {
        [Description("Default")]
        Default = 0,

        [Description("Name A-Z")]
        NameAtoZ = 1,

        [Description("Name Z-A")]
        NameZtoA = 2,

        [Description("Due time")]
        DueTime = 4,

        [Description("Priority high-low")]
        PriorityHighToLow = 8,

        [Description("Priority low-high")]
        PriorityLowToHigh = 16

    }
}

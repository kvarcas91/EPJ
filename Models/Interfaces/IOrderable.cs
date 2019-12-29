using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Interfaces
{
    public interface IOrderable
    {

        /// <summary>
        /// Order number in the list
        /// </summary>
        uint OrderNumber { get; set; }

    }
}

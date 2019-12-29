using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Interfaces
{
    public interface IDateable
    {

        /// <summary>
        /// Submition date. Usually local.now()
        /// </summary>
        DateTime SubmitionDate { get; set; }

        /// <summary>
        /// Deadline
        /// </summary>
        DateTime DueDate { get; set; }

    }
}

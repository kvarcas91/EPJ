using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Interfaces
{
    public interface IElement
    {

       [Key]
        /// <summary>
        /// Element ID
        /// </summary>
        long ID { get; set; }

        /// <summary>
        /// Element Content
        /// </summary>
        string Content { get; set; }

    }
}

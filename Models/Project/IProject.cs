using Dapper.Contrib.Extensions;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Project
{
    public interface IProject : IInteractiveElement, IPath
    {

        /// <summary>
        /// Project Title
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// Is project archived
        /// </summary>
        bool IsArchived { get; set; }

        IList<IElement> Comments { get; set; }

        IList<IContributor> Contributors { get; set; } 
        
        IList<IElement> Tasks { get; set; }
    }
}

using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Task
{
    public interface ISubTask : IInteractiveElement, IOrderable
    {

        bool IsCompleted { get; set; }

        IList<IContributor> Contributors { get; }

    }
}
